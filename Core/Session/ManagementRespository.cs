using LMUSessionTracker.Core.LMU;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Session {
	public interface ManagementRespository {
		public Task<Guid> CreateSession(SessionInfo info);
	}
}
