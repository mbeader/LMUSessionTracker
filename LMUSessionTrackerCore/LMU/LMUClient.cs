using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.LMU {
	public interface LMUClient {
		public Task<List<Chat>> GetChat();

		public Task<MultiplayerTeams> GetMultiplayerTeams();

		public Task<TeamStrategy> GetStrategy();

		public Task<StrategyUsage> GetStrategyUsage();

		public Task<SessionInfo> GetSessionInfo();

		public Task<List<Standing>> GetStandings();

		public Task<StandingsHistory> GetStandingsHistory();

		public Task<List<TrackMapPoint>> GetTrackMap();
	}
}