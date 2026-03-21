namespace LMUSessionTracker.Common.Http {
	public class LMUClientOptions {
		public string BaseUri { get; set; } = "http://localhost:6397/";
		public string WebSocketBaseUri { get; set; } = "ws://localhost:6398/";
		public int TimeoutSeconds { get; set; } = 2;
		public bool ValidateResponses { get; set; } = false;
		public bool LogResponses { get; set; } = false;
		public string LogDirectory { get; set; } = "debug";
	}
}
