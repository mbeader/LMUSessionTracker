namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolStatus {
		public string SessionId { get; set; }
		public ProtocolRole Role { get; set; }
		public ProtocolResult Result { get; set; }
	}
}
