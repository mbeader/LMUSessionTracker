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

		public async Task CreateSession(string sessionId, SessionInfo info, DateTime timestamp) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
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

		public async Task UpdateLaps(string sessionId, List<CarHistory> cars) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				Dictionary<CarKey, Dictionary<int, Lap>> allDbLaps = new Dictionary<CarKey, Dictionary<int, Lap>>();
				foreach(Lap dbLap in await context.Laps.Where(x => x.SessionId == sessionId).ToListAsync()) {
					CarKey key = new CarKey() { SlotId = dbLap.SlotId, Veh = dbLap.Veh };
					if(!allDbLaps.TryGetValue(key, out Dictionary<int, Lap> dbLaps)) {
						dbLaps = new Dictionary<int, Lap>();
						allDbLaps.Add(key, dbLaps);
					}
					dbLaps.Add(dbLap.LapNumber, dbLap);
				}
				foreach(CarHistory car in cars) {
					if(!allDbLaps.TryGetValue(car.Key, out Dictionary<int, Lap> dbLaps)) {
						dbLaps = new Dictionary<int, Lap>();
						allDbLaps.Add(car.Key, dbLaps);
					}
					foreach(var lap in car.Laps) {
						if(lap == null)
							continue;
						if(!dbLaps.TryGetValue(lap.LapNumber, out Lap dbLap)) {
							dbLap = new Lap() { SessionId = sessionId };
							dbLaps.Add(lap.LapNumber, dbLap);
							context.Laps.Add(dbLap);
						}
						dbLap.From(car.Car, lap);
					}
				}
				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
		}

		public async Task UpdateEntries(string sessionId, EntryList entries) {
			using SqliteContext context = await contextFactory.CreateDbContextAsync();
			using(var transaction = await context.Database.BeginTransactionAsync()) {
				foreach(int slotId in entries.Slots.Keys) {
					Entry entry = new Entry() { SessionId = sessionId };
					entry.From(entries.Slots[slotId]);
					context.Entries.Add(entry);
					foreach(Core.Tracking.Member coreMember in entries.Slots[slotId].Members) {
						Member member = new Member() { SessionId = sessionId };
						member.From(coreMember);
						entry.Members.Add(member);
					}
				}
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
				.Include(x => x.Laps)
				.Include(x => x.Entries)
				.Include(x => x.Members)
				.AsSplitQuery()
				.SingleOrDefaultAsync(x => x.SessionId == sessionId);
			return session.To();
		}
	}
}
