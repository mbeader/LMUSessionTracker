using LMUSessionTracker.Core.Tracking;
using System;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class CarHistoryTests {
		private static readonly DateTime dt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private readonly CarHistory history;

		public CarHistoryTests() {
			history = new CarHistory(new CarKey() { SlotId = 0, Veh = "someveh" }, new Car() { SlotId = 0, Veh = "someveh" });
		}

		private void AssertLap(Lap ex, Lap ac) {
			ex.Timestamp = ac.Timestamp;
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void Update_NoLapsCompleted_NoLaps() {
			history.Update(new() { lapsCompleted = 0 }, dt);
			Assert.Equal(0, history.LapsCompleted);
			Assert.Null(history.GetLap(1));
		}

		[Fact]
		public void Update_OneInvalidLapCompleted_OneLap() {
			history.Update(new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, dt);
			Assert.Equal(1, history.LapsCompleted);
			AssertLap(Lap.Default(1), history.GetLap(1));
		}

		[Fact]
		public void Update_OneValidLapCompleted_OneLap() {
			history.Update(new() { lapsCompleted = 1, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, dt);
			Assert.Equal(1, history.LapsCompleted);
			AssertLap(new Lap() { LapNumber = 1, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 }, history.GetLap(1));
		}

		[Fact]
		public void Update_LapSkipped_TwoLaps() {
			history.Update(new() { lapsCompleted = 2, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, dt);
			Assert.Equal(2, history.LapsCompleted);
			AssertLap(Lap.Default(1), history.GetLap(1));
			AssertLap(new Lap() { LapNumber = 2, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 }, history.GetLap(2));
		}

		[Fact]
		public void Update_AddOutOfOrder_TwoLaps() {
			history.Update(new() { lapsCompleted = 2, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, dt);
			history.Update(new() { lapsCompleted = 1, lastSectorTime1 = 11, lastSectorTime2 = 33, lastLapTime = 66 }, dt);
			Assert.Equal(2, history.LapsCompleted);
			AssertLap(new Lap() { LapNumber = 1, Sector1 = 11, Sector2 = 22, Sector3 = 33, TotalTime = 66 }, history.GetLap(1));
			AssertLap(new Lap() { LapNumber = 2, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 }, history.GetLap(2));
		}
	}
}
