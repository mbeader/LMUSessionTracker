using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface ManagementRespository {
		public Task CreateSession(string sessionId, SessionInfo info, DateTime timestamp);
		public Task UpdateSession(string sessionId, SessionInfo info, DateTime timestamp);
		public Task UpdateLaps(string sessionId, List<CarHistory> cars);
		public Task UpdateEntries(string sessionId, EntryList entries);
		public Task<List<Session>> GetSessions();
		public Task<Session> GetSession(string sessionId);
	}
}
