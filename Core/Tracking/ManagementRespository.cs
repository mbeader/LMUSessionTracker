using LMUSessionTracker.Core.LMU;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface ManagementRespository {
		public Task<string> CreateSession(SessionInfo info);
		public Task UpdateSession(string sessionId, SessionInfo info);
	}
}
