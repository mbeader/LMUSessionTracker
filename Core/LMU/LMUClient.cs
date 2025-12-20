using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.LMU {
	public interface LMUClient {
		public void OpenContext();

		public void CloseContext();

		public Task<List<Chat>> GetChat();

		public Task<string> GetMultiplayerJoinState();

		public Task<MultiplayerTeams> GetMultiplayerTeams();

		public Task<ProfileInfo> GetProfileInfo();

		public Task<GameState> GetGameState();

		public Task<SessionsInfoForEvent> GetSessionsInfoForEvent();

		public Task<List<TeamStrategy>> GetStrategy();

		public Task<StrategyUsage> GetStrategyUsage();

		public Task<SessionInfo> GetSessionInfo();

		public Task<List<Standing>> GetStandings();

		public Task<StandingsHistory> GetStandingsHistory();

		public Task<List<TrackMapPoint>> GetTrackMap();
	}
}