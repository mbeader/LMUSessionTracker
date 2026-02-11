using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface ManagementRespository {
		public Task CreateSession(string sessionId, SessionInfo info, DateTime timestamp, bool online);
		public Task UpdateSession(string sessionId, SessionInfo info, DateTime timestamp);
		public Task CloseSession(string sessionId);
		public Task UpdateLaps(string sessionId, List<CarHistory> cars);
		public Task UpdateEntries(string sessionId, EntryList entries);
		public Task TransitionSession(string fromSessionId, string toSessionId);
		public Task<List<Session>> GetSessions();
		public Task<Session> GetSession(string sessionId);
	}
}
