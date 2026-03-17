using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionUpdate {
		public SessionInfo Info { get; set; }
		public List<Standing> Standings { get; set; }
		public MultiplayerTeams Teams { get; set; }
		public List<Chat> Chat { get; set; }
		public List<TeamStrategy> Strategies { get; set; }
		public StrategyUsage Usage { get; set; }

		public SessionUpdate(ProtocolMessage data = null) {
			if(data != null) {
				Info = data.SessionInfo;
				Standings = data.Standings;
				Teams = data.MultiplayerTeams;
				Chat = data.Chat;
				Strategies = data.TeamStrategy;
				Usage = data.StrategyUsage;
			}
		}
	}
}
