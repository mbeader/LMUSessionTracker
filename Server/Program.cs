using LMUSessionTracker.Core;
using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Hubs;
using LMUSessionTracker.Server.Models;
using LMUSessionTracker.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace LMUSessionTracker.Server {
	public class Program {
		public static void Main(string[] args) {
			var logger = ConfigureLogging();
			AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
			logger.Information($"Starting {assembly.Name} {assembly.Version}");
			logger.Information($"Working directory: {Directory.GetCurrentDirectory()}");
			Directory.CreateDirectory("data");

			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers()
				.AddJsonOptions(options => {
					options.JsonSerializerOptions.Converters.Add(new CarKeyConverter());
					options.JsonSerializerOptions.Converters.Add(new CarKeyDictionaryConverter<int>());
					options.JsonSerializerOptions.Converters.Add(new CarKeyDictionaryConverter<Core.Tracking.Car>());
				});
			builder.Services.AddSignalR();

			var clientConfig = builder.Configuration.GetSection("Client");
			var clientOptions = clientConfig.GetSection("Options").Get<ClientOptions>();
			builder.Services.ConfigureClient(clientConfig);
			var serverConfig = builder.Configuration.GetSection("Server");
			var serverOptions = serverConfig.Get<ServerOptions>();
			builder.Services.Configure<ServerOptions>(serverConfig);
			builder.Services.Configure<SchemaValidatorOptions>(builder.Configuration.GetSection("SchemaValidation"));

			builder.Services.AddDbContextFactory<SqliteContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Sqlite"]));

			builder.Services.AddSingleton<DateTimeProvider, DefaultDateTimeProvider>();
			builder.Services.AddSingleton<UuidVersion7Provider, DefaultUuidVersion7Provider>();
			if(builder.Configuration.GetSection("SchemaValidation").GetValue<bool>(nameof(SchemaValidatorOptions.Enabled))) {
				//NewtonsoftSchemaValidator.LoadJsonSchemas();
				//builder.Services.AddScoped<SchemaValidator, NewtonsoftSchemaValidator>();
				SystemTextJsonSchemaValidator.LoadJsonSchemas();
				builder.Services.AddScoped<SchemaValidator, SystemTextJsonSchemaValidator>();
			}
			if(serverOptions.UseLocalClient) {
				ClientInfo clientInfo = new ClientInfo() {
					ClientId = ClientId.LoadOrCreate(clientOptions.PrivateKeyFile),
					OverrideInterval = clientOptions.UseReplay,
					Interval = clientConfig.GetSection("Replay")?.GetValue<int>("Interval"),
					DebugMode = clientOptions.DebugMode,
					TraceLogging = clientOptions.TraceLogging
				};
				builder.Services.AddClient(clientInfo, clientOptions);
			}
			if(serverOptions.RejectAllClients) {
				builder.Services.AddSingleton<ProtocolServer, AutoRejectServer>();
				builder.Services.AddScoped<SessionObserver, DefaultSessionObserver>();
			} else {
				builder.Services.AddSingleton<PublisherService, SignalRPublisherService>();
				builder.Services.AddSingleton<SessionLogger>();
				builder.Services.AddSingleton<SessionArbiter>();
				builder.Services.AddSingleton<ProtocolServer, SessionArbiter>(provider => provider.GetRequiredService<SessionArbiter>());
				builder.Services.AddScoped<SessionObserver, SessionArbiterObserver>();
			}
			builder.Services.AddScoped<SessionRepository, SqliteSessionRepository>();
			builder.Services.AddSingleton<ManagementRespository, SqliteManagementRepository>();
			builder.Services.AddSingleton<ProtocolAuthenticator, DefaultProtocolAuthenticator>();
			builder.Services.AddHostedService<SessionLoaderService>();
			builder.Services.AddHostedService<SessionCleanupService>();


			builder.Logging.ClearProviders();
			builder.Services.AddSerilog((services, lc) => lc
				.ReadFrom.Configuration(builder.Configuration)
				.ReadFrom.Services(services)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId());

			var app = builder.Build();
			using(var serviceScope = app.Services.CreateScope()) {
				var context = serviceScope.ServiceProvider.GetRequiredService<SqliteContext>();
				if(serverOptions.RecreateDatabaseOnStartup) {
					logger.Information("Recreating database");
					context.Database.EnsureDeleted();
				}
				context.Database.Migrate();
			}

			// Configure the HTTP request pipeline.
			app.UseForwardedHeaders(new ForwardedHeadersOptions {
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if(!app.Environment.IsDevelopment()) {
				if(serverOptions.UseHttpsRedirection)
					app.UseHttpsRedirection();
				app.UseHsts();
				app.UseExceptionHandler("/Home/Error");
			}

			var fileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "./wwwroot/browser"));
			app.UseDefaultFiles(new DefaultFilesOptions() { FileProvider = fileProvider });
			app.UseStaticFiles(new StaticFileOptions() { FileProvider = fileProvider });
			app.MapFallbackToFile("index.html", new StaticFileOptions() { FileProvider = fileProvider });

			app.Use(async (context, next) => {
				context.Request.EnableBuffering();
				await next();
			});

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllers();
			app.MapHub<SessionHub>("/api/Live/Session", options => {  });

			app.Run();
		}

		private static Serilog.ILogger ConfigureLogging() {
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
				.Build();
			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.CreateBootstrapLogger();
			Serilog.Context.LogContext.PushProperty("SourceContext", typeof(Program).FullName, false);
			Log.Logger = logger;
			return logger;
		}
	}
}
