using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class HistoryTests {
		private static readonly DateTime dt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private static readonly Car car = new Car() { SlotId = 0, Veh = "someveh", TeamName = "t1" };
		private static readonly CarKey key = new CarKey() { SlotId = 0, Veh = "someveh" };
		private readonly UpdateContextFactory contextFactory;
		private readonly CarStateMonitor state;

		public HistoryTests(LoggingFixture loggingFixture) {
			contextFactory = new UpdateContextFactory(loggingFixture.LoggerFactory);
			state = new CarStateMonitor();
		}

		private UpdateContext<T> Context<T>(DateTime timestamp, double currentET = 0) {
			return contextFactory.Create<T>(timestamp, currentET);
		}

		private Strategy Strategy(int lap, string driver) {
			return new() { lap = lap, driver = driver, tyres = new() { fl = new() { compound = "Medium" } } };
		}

		private Pit ResolvedPit(int lap) {
			return new() { Lap = lap, StopTime = 0, ReleaseTime = 0, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, LFCompound = "Medium", Resolved = true };
		}

		[Fact]
		public void Update_StrategyMatchesCurrentDriver_ResolvesPit() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d1", lapsCompleted = 1 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" } }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(1, "d1") } } }, null);
			AssertHelpers.Equivalent(new() { ResolvedPit(1) }, history.GetAllHistory()[0].Pits);
		}

		[Fact]
		public void Update_StrategyMatchesStrategyLapDriver_ResolvesPit() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d2", lapsCompleted = 1 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" } }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(1, "d1") } } }, null);
			AssertHelpers.Equivalent(new() { ResolvedPit(1) }, history.GetAllHistory()[0].Pits);
		}

		[Fact]
		public void Update_StrategyMatchesPreviousLapDriver_ResolvesPit() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d2", lapsCompleted = 2 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" }, new() { LapNumber = 2, Driver = "d2" } }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(2, "d1") } } }, null);
			AssertHelpers.Equivalent(new() { ResolvedPit(2) }, history.GetAllHistory()[0].Pits);
		}

		[Fact]
		public void Update_StrategyNoMatchLap1_NoPits() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d1", lapsCompleted = 2 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" }, new() { LapNumber = 2, Driver = "d1" } }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(1, "d2") } } }, null);
			Assert.Empty(history.GetAllHistory()[0].Pits);
		}

		[Fact]
		public void Update_StrategyNoMatchLap2_NoPits() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d1", lapsCompleted = 2 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" }, new() { LapNumber = 2, Driver = "d1" } }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(2, "d2") } } }, null);
			Assert.Empty(history.GetAllHistory()[0].Pits);
		}

		[Fact]
		public void Update_StrategyNoMatchLap2Null_NoPits() {
			List<Standing> standings = new() { new() { slotID = 0, vehicleFilename = "someveh", fullTeamName = "t1", driverName = "d1", lapsCompleted = 2 } };
			state.Update(Context<CarStateMonitor>(dt), standings);
			History history = new History(new() { new(key, car, new() { new() { LapNumber = 1, Driver = "d1" }, null }) });
			history.Update(Context<History>(dt), state, standings, new() { new() { Name = "t1", Strategy = new() { Strategy(2, "d2") } } }, null);
			Assert.Empty(history.GetAllHistory()[0].Pits);
		}
	}
}
