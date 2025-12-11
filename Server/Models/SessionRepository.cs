using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public interface SessionRepository {
		public Task<Session> GetSession(Guid sessionId);
		public Task<List<Session>> GetSessions();
	}

	public class SqliteSessionRepository : SessionRepository {
		private readonly ILogger<SqliteSessionRepository> logger;
		private readonly SqliteContext context;

		public SqliteSessionRepository(ILogger<SqliteSessionRepository> logger, SqliteContext context) {
			this.logger = logger;
			this.context = context;
		}

		public Task<Session> GetSession(Guid sessionId) {
			return context.Sessions.SingleAsync(x => x.SessionId == sessionId);
		}

		public Task<List<Session>> GetSessions() {
			return context.Sessions.ToListAsync();
		}
	}
}
