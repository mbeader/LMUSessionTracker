using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class CarStateTests {
		private readonly CarKey key = new CarKey() { SlotId = 0, Veh = "someveh" };

		public CarStateTests() {
		}

		private Standing Standing(Standing standing, Action<Standing> action) {
			if(action != null)
				action(standing);
			return standing;
		}

		private Standing BasicStanding(Action<Standing> action = null) {
			return Standing(new Standing() {
				countLapFlag = "COUNT_LAP_AND_TIME",
				driverName = "driver1",
				finishStatus = "FSTAT_NONE",
				inGarageStall = false,
				lapStartET = 100,
				lapsCompleted = 1,
				penalties = 0,
				pitState = "NONE",
				pitstops = 0,
				pitting = false,
				position = 1,
				serverScored = true,
				sector = "SECTOR2",
				timeIntoLap = 11,
			}, action);
		}

		private Standing EnteringStanding(Action<Standing> action = null) {
			return Standing(new Standing() {
				countLapFlag = "COUNT_LAP_AND_TIME",
				driverName = "driver1",
				finishStatus = "FSTAT_NONE",
				inGarageStall = false,
				lapStartET = 100,
				lapsCompleted = 1,
				penalties = 0,
				pitState = "ENTERING",
				pitstops = 0,
				pitting = true,
				position = 1,
				serverScored = true,
				sector = "SECTOR3",
				timeIntoLap = 57,
			}, action);
		}

		private Standing ExitingStanding(Action<Standing> action = null) {
			return Standing(new Standing() {
				countLapFlag = "COUNT_LAP_AND_TIME",
				driverName = "driver1",
				finishStatus = "FSTAT_NONE",
				inGarageStall = false,
				lapStartET = 100,
				lapsCompleted = 1,
				penalties = 0,
				pitState = "EXITING",
				pitstops = 0,
				pitting = true,
				position = 1,
				serverScored = true,
				sector = "SECTOR3",
				timeIntoLap = 60,
			}, action);
		}

		[Fact]
		public void Next_SameOnTrackState_NoChanges() {
			Assert.Equivalent(new CarState(key, BasicStanding()), new CarState(key, BasicStanding()).Next(BasicStanding()));
		}

		[Fact]
		public void Next_OnTrackToEntering_SetsPitStatus() {
			CarState ex = new CarState(key, EnteringStanding()) { LastPitLap = 2, LastPitTime = 157, PitThisLap = true, TotalPitstops = 1 };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(EnteringStanding()));
		}

		[Fact]
		public void Next_SameEnteringState_KeepsPitStatus() {
			CarState ex = new CarState(key, EnteringStanding()) { LastPitLap = 2, LastPitTime = 157, PitThisLap = true, TotalPitstops = 1 };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(EnteringStanding()).Next(EnteringStanding()));
		}

		[Fact]
		public void Next_NextLapInEnteringState_ResetsLapPitStatus() {
			CarState ex = new CarState(key, EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastPitLap = 2, LastPitTime = 157, PitThisLap = false, TotalPitstops = 1
			};
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(EnteringStanding()).Next(EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })));
		}

		[Fact]
		public void Next_NextLapInExitingState_ResetsLapPitStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastPitLap = 2, LastPitTime = 157, PitThisLap = false, TotalPitstops = 1
			};
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(EnteringStanding()).Next(ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })));
		}

		[Fact]
		public void Next_OnTrackPenalty_SetsPenalty() {
			CarState ex = new CarState(key, BasicStanding(s => s.penalties++)) { TotalPenalties = 1 };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(BasicStanding(s => s.penalties++)));
		}

		[Fact]
		public void Next_OnTrackPenaltyServed_KeepsPenalty() {
			CarState ex = new CarState(key, BasicStanding()) { TotalPenalties = 1 };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(BasicStanding(s => s.penalties++)).Next(BasicStanding()));
		}

		[Fact]
		public void Next_OnTrackToGarage_SetsGarage() {
			CarState ex = new CarState(key, BasicStanding(s => s.inGarageStall = true)) { GarageThisLap = true };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(BasicStanding(s => s.inGarageStall = true)));
		}

		[Fact]
		public void Next_GarageToOnTrack_KeepsGarage() {
			CarState ex = new CarState(key, BasicStanding()) { GarageThisLap = true };
			Assert.Equivalent(ex, new CarState(key, BasicStanding()).Next(BasicStanding(s => s.inGarageStall = true)).Next(BasicStanding()));
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector1_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; })) { LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 1 };
			Assert.Equivalent(ex, new CarState(key, EnteringStanding()).Next(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; })));
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector2_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; })) { LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 2 };
			Assert.Equivalent(ex, new CarState(key, EnteringStanding()).Next(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; })));
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector3_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; })) { LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 3 };
			Assert.Equivalent(ex, new CarState(key, EnteringStanding()).Next(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; })));
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapGarage_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; })) { LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 0, GarageThisLap = true };
			Assert.Equivalent(ex, new CarState(key, EnteringStanding()).Next(ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; })));
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapToOnTrack_KeepsSwapStatus() {
			CarState ex = new CarState(key, BasicStanding(s => s.driverName = "driver2")) { LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 3 };
			Assert.Equivalent(ex, new CarState(key, EnteringStanding()).Next(ExitingStanding(s => s.driverName = "driver2")).Next(BasicStanding(s => s.driverName = "driver2")));
		}
	}
}
