namespace LMUSessionTracker.Core.Client {
	public class ClientOptions {
		public bool LMULoggingOnly { get; set; } = false;
		public bool DebugMode { get; set; } = false;
		public bool UseReplay { get; set; } = false;
		public bool SendReplay { get; set; } = false;
		public string PrivateKeyFile { get; set; }
	}
}
