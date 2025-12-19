using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public interface SessionRepository {
		public Task<Session> GetSession(string sessionId);
		public Task<List<Core.Tracking.SessionSummary>> GetSessions();
		public Task<SessionState> GetSessionState(string sessionId);
	}

	public class SqliteSessionRepository : SessionRepository {
		private readonly ILogger<SqliteSessionRepository> logger;
		private readonly SqliteContext context;

		public SqliteSessionRepository(ILogger<SqliteSessionRepository> logger, SqliteContext context) {
			this.logger = logger;
			this.context = context;
		}

		public async Task<Session> GetSession(string sessionId) {
			return await context.Sessions
				.Include(x => x.LastState)
				.Include(x => x.Cars)
				.ThenInclude(x => x.Laps)
				.Include(x => x.Entries)
				.ThenInclude(x => x.Members)
				.AsSplitQuery()
				.SingleOrDefaultAsync(x => x.SessionId == sessionId);
		}

		public async Task<List<Core.Tracking.SessionSummary>> GetSessions() {
			List<Session> sessions = await context.Sessions.Include(x => x.LastState).OrderByDescending(x => x.Timestamp).ToListAsync();
			// TODO improve this
			var cars = await context.Cars.GroupBy(x => new { x.SessionId }).Select(x => new { x.Key.SessionId, Count = x.Count() }).ToListAsync();
			var entries = await context.Entries.GroupBy(x => new { x.SessionId }).Select(x => new { x.Key.SessionId, Count = x.Count() }).ToListAsync();
			List<Core.Tracking.SessionSummary> summaries = new List<Core.Tracking.SessionSummary>();
			foreach(Session session in sessions) {
				summaries.Add(new Core.Tracking.SessionSummary() {
					SessionId = session.SessionId,
					SecondaryClientIds = new List<string>(),
					Track = session.TrackName,
					Type = session.SessionType,
					Timestamp = session.Timestamp,
					LastUpdate = session.LastState?.Timestamp ?? session.Timestamp,
					Finished = session.IsClosed,
					Active = false,
					CarCount = Math.Max(cars.Find(x => x.SessionId == session.SessionId)?.Count ?? 0, entries.Find(x => x.SessionId == session.SessionId)?.Count ?? 0),
					Remaining = session.LastState?.TimeRemainingInGamePhase ?? 0,
				});
			}
			return summaries;
		}

		public async Task<SessionState> GetSessionState(string sessionId) {
			return await context.SessionStates.SingleOrDefaultAsync(x => x.SessionId == sessionId);
		}
	}
}
