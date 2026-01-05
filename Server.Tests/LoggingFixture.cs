using LMUSessionTracker.Server.Tests;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;

[assembly: CaptureConsole]
[assembly: AssemblyFixture(typeof(LoggingFixture))]

namespace LMUSessionTracker.Server.Tests {
	public class LoggingFixture : IDisposable {
		public ILoggerFactory LoggerFactory { get; private init; }

		public LoggingFixture() {
			LoggerFactory = new SerilogLoggerFactory(ConfigureLogging());
		}

		private static Serilog.ILogger ConfigureLogging() {
			var logger = new LoggerConfiguration()
				.MinimumLevel.Warning()
				.MinimumLevel.Override("LMUSessionTracker.Server", Serilog.Events.LogEventLevel.Debug)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.WriteTo.Console()
				.CreateBootstrapLogger();
			Serilog.Context.LogContext.PushProperty("SourceContext", typeof(LoggingFixture).FullName, false);
			Log.Logger = logger;
			return logger;
		}

		public void Dispose() {
		}
	}
}
