using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public interface ILMUClient {
		public Task<List<Chat>> GetChat();

		public Task<MultiplayerTeams> GetMultiplayerTeams();

		public Task<TeamStrategy> GetStrategy();

		public Task<SessionInfo> GetSessionInfo();

		public Task<List<Standing>> GetStandings();

		public Task<StandingsHistory> GetStandingsHistory();

		public Task<List<TrackMapPoint>> GetTrackMap();
	}
}