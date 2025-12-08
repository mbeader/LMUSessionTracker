using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolMessage {
		public string ClientId { get; set; }
		public string SessionId { get; set; }
		public ProtocolMessageType MessageType { get; set; }
		public SessionInfo SessionInfo { get; set; }
		public MultiplayerTeams MultiplayerTeams { get; set; }
		public List<Standing> Standings { get; set; }
		public TeamStrategy TeamStrategy { get; set; }
		public List<Chat> Chat { get; set; }
	}
}
