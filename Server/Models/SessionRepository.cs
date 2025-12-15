using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public interface SessionRepository {
		public Task<Session> GetSession(string sessionId);
		public Task<List<Session>> GetSessions();
		public Task<SessionState> GetSessionState(string sessionId);
	}

	public class SqliteSessionRepository : SessionRepository {
		private readonly ILogger<SqliteSessionRepository> logger;
		private readonly SqliteContext context;

		public SqliteSessionRepository(ILogger<SqliteSessionRepository> logger, SqliteContext context) {
			this.logger = logger;
			this.context = context;
		}

		public Task<Session> GetSession(string sessionId) {
			return context.Sessions.SingleOrDefaultAsync(x => x.SessionId == sessionId);
		}

		public Task<List<Session>> GetSessions() {
			return context.Sessions.ToListAsync();
		}

		public Task<SessionState> GetSessionState(string sessionId) {
			return context.SessionStates.SingleOrDefaultAsync(x => x.SessionId == sessionId);
		}
	}
}
