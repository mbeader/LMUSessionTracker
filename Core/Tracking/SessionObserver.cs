using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public interface SessionObserver {
		public Task<Session> GetSession(string sessionId);
		public Task<List<SessionSummary>> GetSessions();
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

		public Task<List<SessionSummary>> GetSessions() {
			return arbiter.SummarizeSessions();
		}
	}

	public class DefaultSessionObserver : SessionObserver {
		public Task<Session> GetSession(string sessionId) {
			return Task.FromResult<Session>(null);
		}

		public Task<List<SessionSummary>> GetSessions() {
			return Task.FromResult(new List<SessionSummary>());
		}
	}
}
