using Microsoft.Extensions.Logging;

namespace LMUSessionTracker.Core {
	public class Logger {
		public static ILogger<Logger> Instance { get; set; }

		private Logger() { }
	}
}
