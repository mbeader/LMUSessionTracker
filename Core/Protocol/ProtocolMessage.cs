using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolMessage {
		public string ClientId { get; set; }
		public string SessionId { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ProtocolMessageType MessageType { get; set; }

		public SessionInfo SessionInfo { get; set; }
		public string MultiplayerJoinState { get; set; }
		public GameState GameState { get; set; }

		public MultiplayerTeams MultiplayerTeams { get; set; }
		public List<Standing> Standings { get; set; }
		public List<TeamStrategy> TeamStrategy { get; set; }
		public List<Chat> Chat { get; set; }
	}
}
