using LMUSessionTracker.Core.Client;

namespace LMUSessionTracker.Core {
	public class ClientInfo {
		public ClientId ClientId { get; init; }
		public bool OverrideDelay { get; init; }
		public int? Delay { get; init; }
	}
}
