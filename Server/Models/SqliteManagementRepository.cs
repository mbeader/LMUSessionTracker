using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public class SqliteManagementRepository : ManagementRespository {
		private readonly ILogger<SqliteSessionRepository> logger;
		private readonly IDbContextFactory<SqliteContext> contextFactory;

		public SqliteManagementRepository(ILogger<SqliteSessionRepository> logger, IDbContextFactory<SqliteContext> contextFactory) {
			this.logger = logger;
			this.contextFactory = contextFactory;
		}

		public async Task CreateSession(string sessionId, SessionInfo info, DateTime timestamp, bool online) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			Session session = new Session() { SessionId = sessionId, Timestamp = timestamp, IsOnline = online };
			session.From(info);
			SessionState state = new SessionState() { SessionId = sessionId, Timestamp = timestamp };
			state.From(info);
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				context.Sessions.Add(session);
				context.SessionStates.Add(state);
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task UpdateSession(string sessionId, SessionInfo info, DateTime timestamp) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				SessionState state = await context.SessionStates.SingleAsync(x => x.SessionId == sessionId);
				state.From(info);
				state.Timestamp = timestamp;
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task CloseSession(string sessionId) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				Session session = await context.Sessions.SingleAsync(x => x.SessionId == sessionId);
				session.IsClosed = true;
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task UpdateCarStates(string sessionId, List<Core.Tracking.CarState> cars) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				Dictionary<CarKey, Car> dbCars = new Dictionary<CarKey, Car>();
				foreach(Car dbCar in await context.Cars.Include(x => x.LastState).Where(x => x.SessionId == sessionId).ToListAsync()) {
					CarKey key = new CarKey() { SlotId = dbCar.SlotId, Veh = dbCar.Veh };
					dbCars.Add(key, dbCar);
				}
				foreach(Core.Tracking.CarState car in cars) {
					if(dbCars.TryGetValue(car.Key, out Car dbCar)) {
						if(dbCar.LastState == null) {
							dbCar.LastState = new CarState();
							dbCar.LastState.SessionId = sessionId;
							dbCar.LastState.CarId = dbCar.CarId;
							context.CarStates.Add(dbCar.LastState);
						}
						dbCar.LastState.From(car);
					}
				}
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task UpdateLaps(string sessionId, List<CarHistory> cars) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				Dictionary<CarKey, Car> dbCars = new Dictionary<CarKey, Car>();
				Dictionary<CarKey, Dictionary<int, Lap>> allDbLaps = new Dictionary<CarKey, Dictionary<int, Lap>>();
				Dictionary<CarKey, List<Pit>> allDbPits = new Dictionary<CarKey, List<Pit>>();
				foreach(Car dbCar in await context.Cars.Include(x => x.Laps).Include(x => x.Pits).Where(x => x.SessionId == sessionId).AsSplitQuery().ToListAsync()) {
					CarKey key = new CarKey() { SlotId = dbCar.SlotId, Veh = dbCar.Veh };
					dbCars.Add(key, dbCar);
					Dictionary<int, Lap> dbLaps = new Dictionary<int, Lap>();
					allDbLaps.Add(key, dbLaps);
					foreach(Lap dbLap in dbCar.Laps)
						dbLaps.Add(dbLap.LapNumber, dbLap);
					allDbPits.Add(key, new List<Pit>(dbCar.Pits.OrderBy(x => x.PitTime)));
				}
				int c = 0;
				foreach(CarHistory car in cars) {
					if(!dbCars.TryGetValue(car.Key, out Car dbCar)) {
						dbCar = new Car() { SessionId = sessionId };
						context.Cars.Add(dbCar);
						c++;
					}
					dbCar.From(car.Car);
					if(!allDbLaps.TryGetValue(car.Key, out Dictionary<int, Lap> dbLaps)) {
						dbLaps = new Dictionary<int, Lap>();
						allDbLaps.Add(car.Key, dbLaps);
					}
					foreach(Core.Tracking.Lap lap in car.Laps) {
						if(lap == null)
							continue;
						if(!dbLaps.TryGetValue(lap.LapNumber, out Lap dbLap)) {
							dbLap = new Lap() { SessionId = sessionId };
							dbLaps.Add(lap.LapNumber, dbLap);
							//context.Laps.Add(dbLap);
							dbCar.Laps.Add(dbLap);
						}
						dbLap.From(lap);
					}
					if(!allDbPits.TryGetValue(car.Key, out List<Pit> dbPits)) {
						dbPits = new List<Pit>();
						allDbPits.Add(car.Key, dbPits);
					}
					for(int i = 0; i < car.Pits.Count; i++) {
						Pit dbPit;
						if(i < dbPits.Count)
							dbPit = dbPits[i];
						else {
							dbPit = new Pit();
							dbCar.Pits.Add(dbPit);
						}
						dbPit.From(car.Pits[i]);
					}
				}
				if(c > 0)
					logger.LogDebug($"Added {c} cars");
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task UpdateEntries(string sessionId, EntryList entries) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				Dictionary<CarKey, Car> dbCars = new Dictionary<CarKey, Car>();
				Dictionary<int, List<Entry>> existingEntries = new Dictionary<int, List<Entry>>();
				foreach(Car dbCar in await context.Cars.Include(x => x.Entry).Include(x => x.Entry.Members).Where(x => x.SessionId == sessionId).OrderBy(x => x.CarId).ToListAsync()) {
					CarKey key = new CarKey() { SlotId = dbCar.SlotId, Veh = dbCar.Veh };
					dbCars.Add(key, dbCar);
					if(dbCar.Entry != null) {
						if(!existingEntries.TryGetValue(dbCar.Entry.SlotId, out List<Entry> slotEntries)) {
							slotEntries = new List<Entry>();
							existingEntries.Add(dbCar.Entry.SlotId, slotEntries);
						}
						slotEntries.Add(dbCar.Entry);
					}
				}
				List<int> slots = new List<int>(entries.Slots.Keys);
				slots.Sort();
				int c = 0;
				foreach(int slotId in slots) {
					Entry entry = new Entry() { SessionId = sessionId };
					entry.From(entries.Slots[slotId]);
					foreach(Core.Tracking.Member coreMember in entries.Slots[slotId].Members) {
						Member member = new Member() { SessionId = sessionId };
						member.From(coreMember);
						entry.Members.Add(member);
					}
					if(!(existingEntries.TryGetValue(slotId, out List<Entry> slotEntries) && slotEntries.Exists(x => x.IsSameEntry(entry)))) {
						if(slotEntries != null) {
							logger.LogDebug($"Session {sessionId} slot {slotId} replaced");
						}
						if(dbCars.TryGetValue(new CarKey(entry.SlotId, entry.Vehicle), out Car car) && car.Entry == null)
							car.Entry = entry;
						else
							context.Cars.Add(new Car() { SessionId = sessionId, SlotId = entry.SlotId, Veh = entry.Vehicle, Entry = entry });
						c++;
					}
				}
				if(c > 0)
					logger.LogDebug($"Added {c} cars");
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task TransitionSession(string fromSessionId, string toSessionId) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				if(!await context.SessionTransitions.AnyAsync(x => x.FromSessionId == fromSessionId && x.ToSessionId == toSessionId))
					context.SessionTransitions.Add(new SessionTransition() { FromSessionId = fromSessionId, ToSessionId = toSessionId });
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task<List<Core.Tracking.Session>> GetSessions() {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			List<Core.Tracking.Session> sessions = new List<Core.Tracking.Session>();
			foreach(Session session in await context.Sessions.Include(x => x.LastState).ToListAsync()) {
				sessions.Add(session.To());
			}
			return sessions;
		}

		public async Task<Core.Tracking.Session> GetSession(string sessionId) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			Session session = await context.Sessions
				.Include(x => x.LastState)
				.Include(x => x.Cars)
				.ThenInclude(x => x.Laps)
				.Include(x => x.Cars)
				.ThenInclude(x => x.Pits)
				.Include(x => x.Cars)
				.ThenInclude(x => x.LastState)
				.Include(x => x.Entries)
				.ThenInclude(x => x.Members)
				.AsSplitQuery()
				.SingleOrDefaultAsync(x => x.SessionId == sessionId);
			return session.To();
		}
	}
}
