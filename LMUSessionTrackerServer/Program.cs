using LMUSessionTracker.Core.Http;
using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Session;
using Microsoft.AspNetCore.Builder;
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
			builder.Services.Configure<ServerOptions>(serverConfig);
			builder.Services.Configure<SchemaValidatorOptions>(builder.Configuration.GetSection("SchemaValidation"));

			builder.Services.AddSingleton<SessionManager>();
			builder.Services.AddScoped<SessionViewer>();
			if(builder.Configuration.GetSection("SchemaValidation").GetValue<bool>(nameof(SchemaValidatorOptions.Enabled)))
				builder.Services.AddScoped<SchemaValidation.Validator>();
			if(serverOptions.UseLocalClient) {
				builder.Services.AddScoped<LMUClient, HttpLMUClient>();
				builder.Services.AddScoped<ProtocolClient, HttpProtocolClient>();
				builder.Services.AddHostedService<ClientService>();
			}
			if(serverOptions.RejectAllClients)
				builder.Services.AddSingleton<ProtocolServer, AutoRejectServer>();
			else
				builder.Services.AddSingleton<ProtocolServer, SessionArbiter>();
			//builder.Services.AddHostedService<SessionService>();
			//builder.Services.AddHostedService<ReplayService>();


			builder.Logging.ClearProviders();
			//builder.Logging.AddLog4Net();
			builder.Services.AddSerilog((services, lc) => lc
				.ReadFrom.Configuration(builder.Configuration)
				.ReadFrom.Services(services)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.WriteTo.Console());

			var app = builder.Build();

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
