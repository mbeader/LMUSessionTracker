using LMUSessionTracker.Core;
using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace LMUSessionTracker.Client {
	internal class Program {
		static void Main(string[] args) {
			var logger = ConfigureLogging();
			logger.Information($"Working directory: {Directory.GetCurrentDirectory()}");

			HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings() {
				Args = args,
				ContentRootPath = Directory.GetCurrentDirectory(),
				EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
			});

			var clientConfig = builder.Configuration.GetSection("Client");
			var clientOptions = clientConfig.GetSection("Options").Get<ClientOptions>();
			builder.Services.ConfigureClient(clientConfig);
			builder.Services.Configure<SchemaValidatorOptions>(builder.Configuration.GetSection("SchemaValidation"));

			builder.Services.AddSingleton<DateTimeProvider, DefaultDateTimeProvider>();
			if(builder.Configuration.GetSection("SchemaValidation").GetValue<bool>(nameof(SchemaValidatorOptions.Enabled))) {
				SchemaValidation.LoadJsonSchemas();
				builder.Services.AddScoped<SchemaValidator, SchemaValidation.Validator>();
			}
			ClientInfo clientInfo = new ClientInfo() {
				ClientId = ClientId.LoadOrCreate(clientOptions.PrivateKeyFile),
				OverrideInterval = clientOptions.UseReplay,
				Interval = clientConfig.GetSection("Replay")?.GetValue<int>("Interval")
			};
			builder.Services.AddClient(clientInfo, clientOptions);

			builder.Logging.ClearProviders();
			builder.Services.AddSerilog((services, lc) => lc
				.ReadFrom.Configuration(builder.Configuration)
				.ReadFrom.Services(services)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId());

			var host = builder.Build();

			host.Run();
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
