namespace LMUSessionTracker.Core.Http {
	public class ProtocolClientOptions {
		public string BaseUri { get; set; } = "http://localhost:5000/";
		public int TimeoutSeconds { get; set; } = 30;
	}
}
