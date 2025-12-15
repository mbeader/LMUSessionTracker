using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface ManagementRespository {
		public Task<string> CreateSession(SessionInfo info);
		public Task UpdateSession(string sessionId, SessionInfo info);
		public Task UpdateLaps(string sessionId, List<CarHistory> cars);
	}
}
