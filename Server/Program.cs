using LMUSessionTracker.Core.Http;
using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Replay;
using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace LMUSessionTracker.Server {
	public class Program {
		public static void Main(string[] args) {
			var logger = ConfigureLogging();
			logger.Information($"Working directory: {Directory.GetCurrentDirectory()}");

			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			var clientConfig = builder.Configuration.GetSection("Client");
			var serverConfig = builder.Configuration.GetSection("Server");
			var serverOptions = serverConfig.Get<ServerOptions>();
			builder.Services.Configure<LMUClientOptions>(clientConfig.GetSection("LMU"));
			builder.Services.Configure<ProtocolClientOptions>(clientConfig.GetSection("Protocol"));
			builder.Services.Configure<ReplayOptions>(clientConfig.GetSection("Replay"));
			builder.Services.Configure<ServerOptions>(serverConfig);
			builder.Services.Configure<SchemaValidatorOptions>(builder.Configuration.GetSection("SchemaValidation"));

			//builder.Services.AddDbContext<SqliteContext>(options =>
			//	options.UseSqlite(builder.Configuration["ConnectionStrings:Sqlite"]),
			//	ServiceLifetime.Scoped);
			builder.Services.AddDbContextFactory<SqliteContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Sqlite"]));

			//builder.Services.AddSingleton<SessionManager>();
			//builder.Services.AddScoped<SessionViewer>();
			builder.Services.AddSingleton<DateTimeProvider, DefaultDateTimeProvider>();
			if(builder.Configuration.GetSection("SchemaValidation").GetValue<bool>(nameof(SchemaValidatorOptions.Enabled))) {
				SchemaValidation.LoadJsonSchemas();
				builder.Services.AddScoped<SchemaValidator, SchemaValidation.Validator>();
			}
			if(serverOptions.UseLocalClient) {
				if(serverOptions.UseReplay) {
					builder.Services.AddScoped<ReplayLMUClient>();
					if(serverOptions.SendReplay) {
						builder.Services.AddScoped<LMUClient>(provider => provider.GetRequiredService<ReplayLMUClient>());
						builder.Services.AddScoped<ProtocolClient, HttpProtocolClient>();
						builder.Services.AddSingleton<SimpleContinueProvider<ClientService>>();
						builder.Services.AddSingleton<ContinueProvider<ClientService>>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
						builder.Services.AddSingleton<ContinueProviderSource>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
						builder.Services.AddHostedService<ClientService>();
					} else
						builder.Services.AddHostedService<ReplayClientService>();
				} else {
					builder.Services.AddScoped<LMUClient, HttpLMUClient>();
					builder.Services.AddScoped<ProtocolClient, HttpProtocolClient>();
					if(serverOptions.LMULoggingOnly)
						builder.Services.AddHostedService<ResponseLoggerService>();
					else
						builder.Services.AddHostedService<ClientService>();
				}
			}
			if(serverOptions.RejectAllClients) {
				builder.Services.AddSingleton<ProtocolServer, AutoRejectServer>();
				builder.Services.AddScoped<SessionObserver, DefaultSessionObserver>();
			} else {
				builder.Services.AddSingleton<SessionArbiter>();
				builder.Services.AddSingleton<ProtocolServer, SessionArbiter>(provider => provider.GetRequiredService<SessionArbiter>());
				builder.Services.AddScoped<SessionObserver, SessionArbiterObserver>();
			}
			builder.Services.AddScoped<SessionRepository, SqliteSessionRepository>();
			builder.Services.AddSingleton<ManagementRespository, SqliteManagementRepository>();
			//builder.Services.AddHostedService<SessionService>();
			//builder.Services.AddHostedService<ReplayService>();


			builder.Logging.ClearProviders();
			//builder.Logging.AddLog4Net();
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
			if(!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}

		private static Serilog.ILogger ConfigureLogging() {
			// log4net leftovers
			//var logger = LoggerFactory.Create(config => {
			//	config.AddConsole();
			//	config.AddLog4Net();
			//}).CreateLogger(nameof(Program));
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
