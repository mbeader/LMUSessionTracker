using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LMUSessionTracker.TestClient {
	internal class Program {
		static async Task Main(string[] args) {
			var logger = ConfigureLogging();
			var loggerFactory = new SerilogLoggerFactory(logger);
			logger.Information($"Working directory: {Directory.GetCurrentDirectory()}");

			HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings() {
				Args = args,
				ContentRootPath = Directory.GetCurrentDirectory(),
				EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
			});

			Orchestrator orchestrator = new Orchestrator(builder.Configuration, loggerFactory);
			orchestrator.Configure();
			await orchestrator.Run();
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
