using LMUSessionTracker.Core.Client;

namespace LMUSessionTracker.Core {
	public class ClientInfo {
		public ClientId ClientId { get; init; }
		public bool OverrideInterval { get; init; }
		public int? Interval { get; init; }
	}
}
