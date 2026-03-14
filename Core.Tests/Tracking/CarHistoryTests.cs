using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using static LMUSessionTracker.Common.LMU.PitState;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class CarHistoryTests {
		private static readonly DateTime dt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private static readonly CarKey key = new CarKey() { SlotId = 0, Veh = "someveh" };
		private readonly UpdateContextFactory contextFactory;
		private readonly CarHistory history;
		private readonly CarStateChange state;

		public CarHistoryTests(LoggingFixture loggingFixture) {
			contextFactory = new UpdateContextFactory(loggingFixture.LoggerFactory);
			history = new CarHistory(key, new Car() { SlotId = 0, Veh = "someveh" });
			state = new CarStateChange(new CarState(key));
			state.Next(new CarState(key));
		}

		private UpdateContext<CarHistory> Context(DateTime timestamp, double currentET = 0) {
			return contextFactory.Create<CarHistory>(timestamp, currentET);
		}

		private void AssertLap(Lap ex, Lap ac) {
			ex.Timestamp = ac.Timestamp;
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void Update_NoLapsCompleted_NoLaps() {
			Lap ac = history.Update(Context(dt), state, new() { lapsCompleted = 0 }, null);
			Assert.Equal(0, history.LapsCompleted);
			Assert.Null(ac);
			Assert.Null(history.GetLap(1));
		}

		[Fact]
		public void Update_OneInvalidLapCompleted_OneLap() {
			Lap ex1 = Lap.Default(1);
			ex1.StartTime = 0;
			Lap ac1 = history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			Assert.Equal(1, history.LapsCompleted);
			AssertLap(ex1, ac1);
			Assert.Equal(ac1, history.GetLap(1));
		}

		[Fact]
		public void Update_OneValidLapCompleted_OneLap() {
			Lap ex1 = new Lap() { LapNumber = 1, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 };
			Lap ac1 = history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, null);
			Assert.Equal(1, history.LapsCompleted);
			AssertLap(ex1, ac1);
			Assert.Equal(ac1, history.GetLap(1));
		}

		[Fact]
		public void Update_LapSkipped_TwoLaps() {
			Lap ex1 = Lap.Default(1);
			Lap ex2 = new Lap() { LapNumber = 2, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 };
			Lap ac2 = history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, null);
			Assert.Equal(2, history.LapsCompleted);
			AssertLap(ex1, history.GetLap(1));
			AssertLap(ex2, ac2);
			Assert.Equal(ac2, history.GetLap(2));
		}

		[Fact]
		public void Update_AddOutOfOrder_TwoLaps() {
			Lap ex1 = new Lap() { LapNumber = 1, Sector1 = 11, Sector2 = 22, Sector3 = 33, TotalTime = 66 };
			Lap ex2 = new Lap() { LapNumber = 2, Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 };
			Lap ac2 = history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = 10, lastSectorTime2 = 30, lastLapTime = 60 }, null);
			Lap ac1 = history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = 11, lastSectorTime2 = 33, lastLapTime = 66 }, null);
			Assert.Equal(2, history.LapsCompleted);
			AssertLap(ex1, ac1);
			AssertLap(ex2, ac2);
			Assert.Equal(ac1, history.GetLap(1));
			Assert.Equal(ac2, history.GetLap(2));
		}

		[Fact]
		public void Update_PitLap2_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2Sequence_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, ExitTime = 40 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2CrossLineWhileExitingSequence_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, LastPitLap = 2,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, LastPitLap = 2, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, LastPitLap = 2, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, LastPitLap = 2, LastLapEndPitState = EXITING, ThisLapStartPitState = EXITING,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, LastPitLap = 2, LastLapEndPitState = EXITING, ThisLapStartPitState = EXITING,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, ExitTime = 40 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopSwapLap2_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, SwapThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastSwapTime = 25, SwapLocation = 3 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, SwapTime = 25, SwapLocation = 3, Swap = true } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopSwapLap2Sequence_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, SwapThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastSwapTime = 25, SwapLocation = 3 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, SwapThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastSwapTime = 25, SwapLocation = 3 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, SwapThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40, LastSwapTime = 25, SwapLocation = 3 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, ExitTime = 40, SwapTime = 25, SwapLocation = 3, Swap = true } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2SameState_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2And3_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, PitThisLap = true, StopThisLap = true,
				LastPitTime = 40, LastStopTime = 50, LastReleaseTime = 60 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30 }, new() { Lap = 3, PitTime = 40, StopTime = 50, ReleaseTime = 60 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2StopAfterLine_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, StopThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, StopAfterLine = true } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2ReleaseLap3_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, LastLapEndPitState = STOPPED, ThisLapStartPitState = EXITING,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, LastLapEndPitState = STOPPED, ThisLapStartPitState = EXITING,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, ExitTime = 40 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2And3StopAfterLine_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, StopThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, PitThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 40, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 3, StopThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 40, LastStopTime = 50 });
			history.Update(Context(dt), state, new() { lapsCompleted = 3, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, StopAfterLine = true }, new() { Lap = 3, PitTime = 40, StopTime = 50, StopAfterLine = true } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2AndPitLap3StopAfterLine_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true,
				LastPitTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, StopThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 10, LastStopTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, StopThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 25 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 2, PitThisLap = true, LastLapEndPitState = ENTERING,
				LastPitTime = 40, LastStopTime = 20, LastReleaseTime = 25 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 3, LastLapEndPitState = ENTERING,
				LastPitTime = 40, LastStopTime = 20, LastReleaseTime = 25 });
			history.Update(Context(dt), state, new() { lapsCompleted = 3, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 25, StopAfterLine = true }, new() { Lap = 3, PitTime = 40, StopAfterLine = true } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2ThenEnterAgainSameLap_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40, });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 50, LastStopTime = 20, LastReleaseTime = 30, LastExitTime = 40, });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30, ExitTime = 40 }, new() { Lap = 2, PitTime = 50 } }, history.Pits);
		}

		[Fact]
		public void Update_GarageLap2_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, GarageInTime = 10 } }, history.Pits);
		}

		[Fact]
		public void Update_GarageLap2ThenSwap_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true, SwapThisLap = true, SwapLocation = 0,
				LastGarageInTime = 10, LastSwapTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, GarageInTime = 10, SwapTime = 20, Swap = true, SwapLocation = 0 } }, history.Pits);
		}

		[Fact]
		public void Update_GarageLap2ThenExitThenSwap_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10, LastGarageOutTime = 15 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true, SwapThisLap = true, SwapLocation = 0,
				LastGarageInTime = 10, LastGarageOutTime = 15, LastSwapTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, GarageInTime = 10, GarageOutTime = 15 } }, history.Pits);
		}

		[Fact]
		public void Update_GarageLap2SuccessiveWithPitSet_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10, LastPitTime = 9 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, GarageInTime = 10 } }, history.Pits);
		}

		[Fact]
		public void Update_PitStopLap2ThenGarage_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, GarageThisLap = true,
				LastPitTime = 10, LastStopTime = 20, LastReleaseTime = 30, LastGarageInTime = 40 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, PitTime = 10, StopTime = 20, ReleaseTime = 30 }, new() { Lap = 2, GarageInTime = 40 } }, history.Pits);
		}

		[Fact]
		public void Update_StopLap2ThenGarage_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true,
				LastStopTime = 20, LastReleaseTime = 30 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, PitThisLap = true, StopThisLap = true, GarageThisLap = true,
				LastStopTime = 20, LastReleaseTime = 30, LastGarageInTime = 40 });
			history.Update(Context(dt), state, new() { lapsCompleted = 2, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, StopTime = 20, ReleaseTime = 30 }, new() { Lap = 2, GarageInTime = 40 } }, history.Pits);
		}

		[Fact]
		public void Update_GarageLap2ThenEnterAgainSameLap_TwoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 10, LastGarageOutTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			state.Next(new CarState(key) { LapsCompleted = 1, GarageThisLap = true,
				LastGarageInTime = 30, LastGarageOutTime = 20 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 }, null);
			AssertHelpers.Equivalent(new() { new() { Lap = 2, GarageInTime = 10, GarageOutTime = 20 }, new() { Lap = 2, GarageInTime = 30 } }, history.Pits);
		}

		private Strategy Tires(Strategy strategy, StrategyTire tire) {
			strategy.tyres = new() { fl = tire, fr = tire, rl = tire, rr = tire };
			return strategy;
		}

		private Pit Tires(Pit pit, string compound, bool changed, bool notUsed, double usage = -1) {
			pit.LFCompound = compound;
			pit.LFChanged = changed;
			pit.LFNew = notUsed;
			pit.LFUsage = usage;
			pit.RFCompound = compound;
			pit.RFChanged = changed;
			pit.RFNew = notUsed;
			pit.RFUsage = usage;
			pit.LRCompound = compound;
			pit.LRChanged = changed;
			pit.LRNew = notUsed;
			pit.LRUsage = usage;
			pit.RRCompound = compound;
			pit.RRChanged = changed;
			pit.RRNew = notUsed;
			pit.RRUsage = usage;
			return pit;
		}

		[Fact]
		public void Update_NoPitsNullTires_NoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { new() });
			AssertHelpers.Equivalent(new() { }, history.Pits);
		}

		[Fact]
		public void Update_NoPitsDefaultStrategy_NoPits() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { new() { tyres = new() { fl = new() { compound = "N/A" } } } });
			AssertHelpers.Equivalent(new() { }, history.Pits);
		}

		[Fact]
		public void Update_NoPitsInitialStrategy_OnePit() {
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { Tires(new() { lap = 0 }, new() { compound = "Medium", New = true }) });
			AssertHelpers.Equivalent(new() {
				Tires(new() { Lap = 0, GarageOutTime = 0, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, Resolved = true}, "Medium", false, true)
			}, history.Pits);
		}

		[Fact]
		public void Update_OnePitInitialStrategy_OnePit() {
			history.Pits.Add(new Pit() { Lap = 1, GarageInTime = 0 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { Tires(new() { lap = 0 }, new() { compound = "Medium", New = true }) });
			AssertHelpers.Equivalent(new() {
				Tires(new() { Lap = 1, GarageInTime = 0, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, Resolved = true }, "Medium", false, true)
			}, history.Pits);
		}

		[Fact]
		public void Update_OnePitMatchedStrategy_OnePit() {
			history.Pits.Add(new Pit() { Lap = 2, StopTime = 10 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { Tires(new() { lap = 2 }, new() { compound = "Medium", New = true }) });
			AssertHelpers.Equivalent(new() {
				Tires(new() { Lap = 2, StopTime = 10, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, Resolved = true }, "Medium", false, true)
			}, history.Pits);
		}

		[Fact]
		public void Update_OnePitStopAfterLineMatchedStrategy_OnePit() {
			history.Pits.Add(new Pit() { Lap = 2, StopTime = 10, StopAfterLine = true });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { Tires(new() { lap = 3 }, new() { compound = "Medium", New = true }) });
			AssertHelpers.Equivalent(new() {
				Tires(new() { Lap = 2, StopTime = 10, StopAfterLine = true, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, Resolved = true }, "Medium", false, true)
			}, history.Pits);
		}

		[Fact]
		public void Update_OnePitUnmatchedStrategy_TwoPits() {
			history.Pits.Add(new Pit() { Lap = 2, GarageOutTime = 0 });
			history.Update(Context(dt), state, new() { lapsCompleted = 1, lastSectorTime1 = -1, lastSectorTime2 = -1, lastLapTime = -1 },
				new() { Tires(new() { lap = 3 }, new() { compound = "Medium", New = true }) });
			AssertHelpers.Equivalent(new() {
				new() { Lap = 2, GarageOutTime = 0 },
				Tires(new() { Lap = 3, VirtualEnergy = 0, PreviousStintDuration = 0, Time = 0, Resolved = true }, "Medium", false, true)
			}, history.Pits);
		}
	}
}
