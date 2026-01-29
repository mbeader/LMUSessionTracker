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
		public Task<List<Lap>> GetResults(string sessionId);
		public Task<List<Lap>> GetTimedResults(string sessionId);
		public Task<List<string>> GetTracks();
		public Task<List<BestLap>> GetLaps(BestLapsFilters filters);
		public Task<Dictionary<string, ClassBest>> GetClassBests(BestLapsFilters filters);
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
				.Join(context.Cars.GroupBy(x => x.SessionId).Select(x => x.Key), x => new { x.SessionId }, x => new { SessionId = x }, (x, y) => x)
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

		public async Task<List<Lap>> GetResults(string sessionId) {
			return await context.Laps.Include(x => x.Car).Include(x => x.Car.Entry)
				.Where(x => x.SessionId == sessionId)
				.Join(
					context.Laps
						.Where(x => x.SessionId == sessionId)
						.GroupBy(x => x.CarId)
						.Select(x => new { CarId = x.Key, LapNumber = x.Max(x => x.LapNumber) }),
					x => new { x.CarId, x.LapNumber }, x => x, (x, y) => x)
				.OrderByDescending(x => x.FinishStatus != "FSTAT_DQ").ThenByDescending(x => x.LapNumber).ThenBy(x => x.Position)
				.ToListAsync();
		}

		public async Task<List<Lap>> GetTimedResults(string sessionId) {
			var laps = await context.Laps.Include(x => x.Car).Include(x => x.Car.Entry)
				.Where(x => x.SessionId == sessionId)
				.Join(
					context.Laps
						.Where(x => x.SessionId == sessionId && x.TotalTime > 0 && x.IsValid)
						.GroupBy(x => x.CarId)
						.Select(x => new { CarId = x.Key, TotalTime = x.Min(x => x.TotalTime), TotalLaps = x.Max(x => x.LapNumber) }),
					x => new { x.CarId, x.TotalTime }, x => new { x.CarId, x.TotalTime }, (x, y) => new { Lap = x, y.TotalLaps })
				.OrderBy(x => x.Lap.TotalTime)
				.ToListAsync();
			List<Lap> res = new List<Lap>();
			foreach(var lap in laps) {
				lap.Lap.LapNumber = lap.TotalLaps;
				res.Add(lap.Lap);
			}
			return res;
		}

		public async Task<List<string>> GetTracks() {
			return await context.Sessions.GroupBy(x => x.TrackName).Select(x => x.Key).OrderBy(x => x).ToListAsync();
		}

		public async Task<List<BestLap>> GetLaps(BestLapsFilters filters) {
			HashSet<string> knownDrivers = context.KnownDrivers.Select(x => x.Name).ToHashSet();

			// queries for best lap and each best sector are all the same, only the field is different
			// however, I do not understand how to successfully parameterize by a field selector expression
			var bs1Query = LapQuery(filters, knownDrivers)
				.Where(x => x.Sector1 > 0)
				.GroupBy(x => new { x.Driver, x.Car.Veh })
				.Select(x => new { x.Key.Driver, x.Key.Veh, Time = x.Min(x => x.Sector1) })
				.Join(context.Laps.Include(x => x.Car), x => new { x.Driver, x.Veh, x.Time }, x => new { x.Driver, x.Car.Veh, Time = x.Sector1 }, (x, y) => y)
				.GroupBy(x => new { x.Driver, x.Car.Veh, x.Sector1 })
				.Select(x => new { x.Key.Driver, x.Key.Veh, x.Key.Sector1, LapId = x.Select(x => x.LapId).Min() })
				.Join(context.Laps.Include(x => x.Car), x => x.LapId, x => x.LapId, (x, y) => y)
				.OrderBy(x => x.Sector1);
			var bs2Query = LapQuery(filters, knownDrivers)
				.Where(x => x.Sector2 > 0)
				.GroupBy(x => new { x.Driver, x.Car.Veh })
				.Select(x => new { x.Key.Driver, x.Key.Veh, Time = x.Min(x => x.Sector2) })
				.Join(context.Laps.Include(x => x.Car), x => new { x.Driver, x.Veh, x.Time }, x => new { x.Driver, x.Car.Veh, Time = x.Sector2 }, (x, y) => y)
				.GroupBy(x => new { x.Driver, x.Car.Veh, x.Sector2 })
				.Select(x => new { x.Key.Driver, x.Key.Veh, x.Key.Sector2, LapId = x.Select(x => x.LapId).Min() })
				.Join(context.Laps.Include(x => x.Car), x => x.LapId, x => x.LapId, (x, y) => y)
				.OrderBy(x => x.Sector2);
			var bs3Query = LapQuery(filters, knownDrivers)
				.Where(x => x.Sector3 > 0)
				.GroupBy(x => new { x.Driver, x.Car.Veh })
				.Select(x => new { x.Key.Driver, x.Key.Veh, Time = x.Min(x => x.Sector3) })
				.Join(context.Laps.Include(x => x.Car), x => new { x.Driver, x.Veh, x.Time }, x => new { x.Driver, x.Car.Veh, Time = x.Sector3 }, (x, y) => y)
				.GroupBy(x => new { x.Driver, x.Car.Veh, x.Sector3 })
				.Select(x => new { x.Key.Driver, x.Key.Veh, x.Key.Sector3, LapId = x.Select(x => x.LapId).Min() })
				.Join(context.Laps.Include(x => x.Car), x => x.LapId, x => x.LapId, (x, y) => y)
				.OrderBy(x => x.Sector3);
			var laps = await LapQuery(filters, knownDrivers)
				.GroupBy(x => new { x.Driver, x.Car.Veh })
				.Select(x => new { x.Key.Driver, x.Key.Veh, Time = x.Min(x => x.TotalTime) })
				.Join(context.Laps.Include(x => x.Car), x => new { x.Driver, x.Veh, x.Time }, x => new { x.Driver, x.Car.Veh, Time = x.TotalTime }, (x, y) => y)
				.GroupBy(x => new { x.Driver, x.Car.Veh, x.TotalTime })
				.Select(x => new { x.Key.Driver, x.Key.Veh, x.Key.TotalTime, LapId = x.Select(x => x.LapId).Min() })
				.Join(context.Laps.Include(x => x.Car), x => x.LapId, x => x.LapId, (x, y) => y)
				.LeftJoin(bs1Query, x => new { x.Driver, x.Car.Veh }, x => new { x.Driver, x.Car.Veh }, (x, y) => new { Lap = x, S1 = y })
				.LeftJoin(bs2Query, x => new { x.Lap.Driver, x.Lap.Car.Veh }, x => new { x.Driver, x.Car.Veh }, (x, y) => new { x.Lap, x.S1, S2 = y })
				.LeftJoin(bs3Query, x => new { x.Lap.Driver, x.Lap.Car.Veh }, x => new { x.Driver, x.Car.Veh }, (x, y) => new { x.Lap, x.S1, x.S2, S3 = y })
				.OrderBy(x => x.Lap.TotalTime)
				.Take(1000)
				.ToListAsync();

			List<BestLap> res = new List<BestLap>();
			foreach(var lap in laps) {
				BestLap best = new BestLap() { Lap = lap.Lap, Sector1 = lap.S1, Sector2 = lap.S2, Sector3 = lap.S3 };
				res.Add(best);
				best.Lap.Known = knownDrivers.Contains(best.Lap.Driver);
			}
			return res;
		}

		/// <summary>
		/// Query that returns all laps matching the given filters
		/// </summary>
		private IQueryable<Lap> LapQuery(BestLapsFilters filters, HashSet<string> knownDrivers) {
			bool hasUnknown = filters.Classes.Contains("Unknown");
			return context.Sessions
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
				.OrderBy(x => x.TotalTime);
		}

		public async Task<Dictionary<string, ClassBest>> GetClassBests(BestLapsFilters filters) {
			HashSet<string> knownDrivers = context.KnownDrivers.Select(x => x.Name).ToHashSet();
			var bests = await LapQuery(filters, knownDrivers)
				.GroupBy(x => new { x.Car.Class })
				.Select(x => new {
					x.Key,
					Bests = new ClassBest() {
						TotalTime = x.Min(x => x.TotalTime),
						Sector1 = ((double?)x.Select(x => x.Sector1).Where(x => x > 0).Min()) ?? -3,
						Sector2 = ((double?)x.Select(x => x.Sector2).Where(x => x > 0).Min()) ?? -3,
						Sector3 = ((double?)x.Select(x => x.Sector3).Where(x => x > 0).Min()) ?? -3
					}
				})
				.ToDictionaryAsync(x => x.Key.Class, x => x.Bests);
			return bests;
		}
	}
}
