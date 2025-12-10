namespace LMUSessionTracker.Server {
	public class ServerOptions {
		public bool UseLocalClient { get; set; } = false;
		public bool RejectAllClients { get; set; } = false;
		public bool LMULoggingOnly { get; set; } = false;
		public bool UseReplay { get; set; } = false;
	}
}
