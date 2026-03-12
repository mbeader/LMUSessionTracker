using LMUSessionTracker.Common.Client;

namespace LMUSessionTracker.Common {
	public class ClientInfo {
		public ClientId ClientId { get; init; }
		public bool OverrideInterval { get; init; }
		public int? Interval { get; init; }
		public bool DebugMode { get; init; }
		public bool TraceLogging { get; init; }
	}
}
