using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Replay {
	public class ReplayRun {
		public List<Chat> Chat { get; set; }
		public string MultiplayerJoinState { get; set; }
		public MultiplayerTeams MultiplayerTeams { get; set; }
		public ProfileInfo ProfileInfo { get; set; }
		public GameState GameState { get; set; }
		public SessionsInfoForEvent SessionsInfoForEvent { get; set; }
		public List<TeamStrategy> Strategy { get; set; }
		public StrategyUsage StrategyUsage { get; set; }
		public SessionInfo SessionInfo { get; set; }
		public List<Standing> Standings { get; set; }
		public StandingsHistory StandingsHistory { get; set; }
		public List<TrackMapPoint> TrackMap { get; set; }
	}
}
