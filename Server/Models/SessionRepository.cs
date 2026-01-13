using LMUSessionTracker.Server.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public interface SessionRepository {
		public Task<Session> GetSession(string sessionId);
		public Task<int> GetSessionCount();
		public Task<List<Core.Tracking.SessionSummary>> GetSessions(int page, int pageSize);
		public Task<SessionState> GetSessionState(string sessionId);
		public Task<List<Car>> GetEntries(string sessionId);
		public Task<List<string>> GetTracks();
		public Task<List<Lap>> GetLaps(BestLapsFilters filters);
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

		public async Task<int> GetSessionCount() {
			return await context.Sessions.CountAsync();
		}

		public async Task<List<Core.Tracking.SessionSummary>> GetSessions(int page, int pageSize) {
			List<Session> sessions = await context.Sessions.Include(x => x.LastState)
				.OrderByDescending(x => x.Timestamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			// TODO improve this
			var cars = await context.Cars.GroupBy(x => new { x.SessionId }).Select(x => new { x.Key.SessionId, Count = x.Count() }).ToListAsync();
			var entries = await context.Entries.GroupBy(x => new { x.SessionId }).Select(x => new { x.Key.SessionId, Count = x.Count() }).ToListAsync();
			var laps = await context.Laps.GroupBy(x => new { x.SessionId }).Select(x => new { x.Key.SessionId, Count = x.Count() }).ToListAsync();
			List<Core.Tracking.SessionSummary> summaries = new List<Core.Tracking.SessionSummary>();
			foreach(Session session in sessions) {
				summaries.Add(new Core.Tracking.SessionSummary() {
					SessionId = session.SessionId,
					SecondaryClientIds = new List<string>(),
					Track = session.TrackName,
					Type = session.SessionType,
					Online = session.IsOnline,
					Timestamp = session.Timestamp,
					LastUpdate = session.LastState?.Timestamp ?? session.Timestamp,
					Finished = session.IsClosed,
					Active = false,
					CarCount = Math.Max(cars.Find(x => x.SessionId == session.SessionId)?.Count ?? 0, entries.Find(x => x.SessionId == session.SessionId)?.Count ?? 0),
					LapCount = laps.Find(x => x.SessionId == session.SessionId)?.Count ?? 0,
					Remaining = session.LastState?.TimeRemainingInGamePhase ?? 0,
					Phase = session.LastState?.GamePhase ?? -1,
				});
			}
			return summaries;
		}

		public async Task<SessionState> GetSessionState(string sessionId) {
			return await context.SessionStates.SingleOrDefaultAsync(x => x.SessionId == sessionId);
		}

		public async Task<List<Car>> GetEntries(string sessionId) {
			return await context.Cars.Include(x => x.Entry).Include(x => x.Entry.Members).Where(x => x.SessionId == sessionId).OrderBy(x => x.CarId).ToListAsync();
		}

		public async Task<List<string>> GetTracks() {
			return await context.Sessions.GroupBy(x => x.TrackName).Select(x => x.Key).OrderBy(x => x).ToListAsync();
		}

		public async Task<List<Lap>> GetLaps(BestLapsFilters filters) {
			bool hasUnknown = filters.Classes.Contains("Unknown");
			HashSet<string> knownDrivers = context.KnownDrivers.Select(x => x.Name).ToHashSet();
			List<Lap> laps = await context.Sessions
				.Where(x =>
					x.TrackName == filters.Track &&
					(!filters.OnlineOnly.HasValue || x.IsOnline == filters.OnlineOnly.Value) &&
					filters.SessionTypes.Contains(x.SessionType)
				)
				.Join(context.Laps.Include(x => x.Car), x => x.SessionId, x => x.SessionId, (x, y) => y)
				.Where(x =>
					x.TotalTime > 0 &&
					x.IsValid &&
					(!filters.Since.HasValue || (x.Timestamp.HasValue && filters.Since.Value <= x.Timestamp.Value)) &&
					 (
						(
							(x.Car.Class == "Hyper" || x.Car.Class == "LMP2" || x.Car.Class == "LMP2_ELMS" || x.Car.Class == "LMP3" || x.Car.Class == "GTE" || x.Car.Class == "GT3") &&
							filters.Classes.Contains(x.Car.Class)
						) ||
						(
							!(x.Car.Class == "Hyper" || x.Car.Class == "LMP2" || x.Car.Class == "LMP2_ELMS" || x.Car.Class == "LMP3" || x.Car.Class == "GTE" || x.Car.Class == "GT3") &&
							hasUnknown
						)
					) &&
					(!filters.KnownDriversOnly || knownDrivers.Contains(x.Driver))
				)
				.OrderBy(x => x.TotalTime)
				.GroupBy(x => new { x.Driver, x.Car.Veh })
				.Select(x => new { x.Key.Driver, x.Key.Veh, TotalTime = x.Select(x => x.TotalTime).Min() })
				.Join(context.Laps.Include(x => x.Car), x => new { x.Driver, x.Veh, x.TotalTime }, x => new { x.Driver, x.Car.Veh, x.TotalTime }, (x, y) => y)
				.GroupBy(x => new { x.Driver, x.Car.Veh, x.TotalTime })
				.Select(x => new { x.Key.Driver, x.Key.Veh, x.Key.TotalTime, LapId = x.Select(x => x.LapId).Min() })
				.Join(context.Laps.Include(x => x.Car), x => x.LapId, x => x.LapId, (x, y) => y)
				.OrderBy(x => x.TotalTime)
				.Take(100)
				.ToListAsync();
			foreach(Lap lap in laps)
				lap.Known = knownDrivers.Contains(lap.Driver);
			return laps;
		}
	}
}
