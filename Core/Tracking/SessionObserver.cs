using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface SessionObserver {
		public Task<Session> GetSession(string sessionId);
	}

	public class SessionArbiterObserver : SessionObserver {
		private readonly ILogger<SessionArbiterObserver> logger;
		private readonly SessionArbiter arbiter;

		public SessionArbiterObserver(ILogger<SessionArbiterObserver> logger, SessionArbiter arbiter) {
			this.logger = logger;
			this.arbiter = arbiter;
		}

		public Task<Session> GetSession(string sessionId) {
			return arbiter.CloneSession(sessionId);
		}
	}

	public class DefaultSessionObserver : SessionObserver {
		public Task<Session> GetSession(string sessionId) {
			return Task.FromResult<Session>(null);
		}
	}
}
