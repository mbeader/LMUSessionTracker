using Microsoft.Extensions.Logging;

namespace LMUSessionTracker.Core.Tracking {
	public interface SessionObserver {
		public Session GetSession(string sessionId);
	}

	public class SessionArbiterObserver : SessionObserver {
		private readonly ILogger<SessionArbiterObserver> logger;
		private readonly SessionArbiter arbiter;

		public SessionArbiterObserver(ILogger<SessionArbiterObserver> logger, SessionArbiter arbiter) {
			this.logger = logger;
			this.arbiter = arbiter;
		}

		public Session GetSession(string sessionId) {
			return arbiter.CloneSession(sessionId);
		}
	}

	public class DefaultSessionObserver : SessionObserver {
		public Session GetSession(string sessionId) {
			return null;
		}
	}
}
