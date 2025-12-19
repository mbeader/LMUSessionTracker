using LMUSessionTracker.Core;
using LMUSessionTracker.Core.Http;
using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Replay;
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
			builder.Services.Configure<ClientOptions>(clientConfig.GetSection("Options"));
			builder.Services.Configure<LMUClientOptions>(clientConfig.GetSection("LMU"));
			builder.Services.Configure<ProtocolClientOptions>(clientConfig.GetSection("Protocol"));
			builder.Services.Configure<ReplayOptions>(clientConfig.GetSection("Replay"));
			builder.Services.Configure<SchemaValidatorOptions>(builder.Configuration.GetSection("SchemaValidation"));

			if(builder.Configuration.GetSection("SchemaValidation").GetValue<bool>(nameof(SchemaValidatorOptions.Enabled))) {
				SchemaValidation.LoadJsonSchemas();
				builder.Services.AddScoped<SchemaValidator, SchemaValidation.Validator>();
			}
			builder.Services.AddSingleton<ClientInfo>(new ClientInfo() { ClientId = "t" });
			if(clientOptions.UseReplay) {
				builder.Services.AddScoped<ReplayLMUClient>();
				if(clientOptions.SendReplay) {
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
				if(clientOptions.LMULoggingOnly)
					builder.Services.AddHostedService<ResponseLoggerService>();
				else
					builder.Services.AddHostedService<ClientService>();
			}

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
