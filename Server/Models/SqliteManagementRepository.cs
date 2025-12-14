using LMUSessionTracker.Core;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public class SqliteManagementRepository : ManagementRespository {
		private readonly ILogger<SqliteSessionRepository> logger;
		private readonly IDbContextFactory<SqliteContext> contextFactory;

		public SqliteManagementRepository(ILogger<SqliteSessionRepository> logger, IDbContextFactory<SqliteContext> contextFactory) {
			this.logger = logger;
			this.contextFactory = contextFactory;
		}

		public async Task<Guid> CreateSession(SessionInfo info) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			Guid sessionId = GuidHelpers.CreateVersion7();
			DateTime timestamp = DateTime.UtcNow;
			Session session = new Session() { SessionId = sessionId, Timestamp = timestamp };
			session.From(info);
			SessionState state = new SessionState() { SessionId = sessionId, Timestamp = timestamp };
			state.From(info);
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				context.Sessions.Add(session);
				context.SessionStates.Add(state);
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			return sessionId;
		}

		public async Task UpdateSession(Guid sessionId, SessionInfo info) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			DateTime timestamp = DateTime.UtcNow;
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				SessionState state = await context.SessionStates.SingleAsync(x => x.SessionId == sessionId);
				state.From(info);
				state.Timestamp = timestamp;
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}
	}
}
