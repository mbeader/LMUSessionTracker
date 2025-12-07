using LMUSessionTracker.Core.LMU;

namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolMessage {
		public string ClientId { get; set; }
		public string SessionId { get; set; }
		public ProtocolMessageType MessageType { get; set; }
		public SessionInfo SessionInfo { get; set; }
		public MultiplayerTeams MultiplayerTeams { get; set; }
	}
}
