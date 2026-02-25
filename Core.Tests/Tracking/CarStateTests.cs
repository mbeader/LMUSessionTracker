using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;

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

		private Standing StoppedStanding(Action<Standing> action = null) {
			return Standing(new Standing() {
				countLapFlag = "COUNT_LAP_AND_TIME",
				driverName = "driver1",
				finishStatus = "FSTAT_NONE",
				inGarageStall = false,
				lapStartET = 100,
				lapsCompleted = 1,
				penalties = 0,
				pitState = "STOPPED",
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

		private void AssertCarState(CarState ex, List<Standing> standingSequence) {
			CarState ac = new CarState(key, standingSequence[0]);
			// timeIntoLap stops increasing when in the pits for some reason, but it is used here for the tests
			for(int i = 1; i < standingSequence.Count; i++)
				ac = ac.Next(standingSequence[i].lapStartET + standingSequence[i].timeIntoLap, standingSequence[i]);
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void Next_SameOnTrackState_NoChanges() {
			AssertCarState(new CarState(key, BasicStanding()), new List<Standing>() { BasicStanding(), BasicStanding() });
		}

		[Fact]
		public void Next_OnTrackToEntering_SetsPitStatus() {
			CarState ex = new CarState(key, EnteringStanding()) { LastPitLap = 2, LastPitTime = 157, PitThisLap = true, TotalPits = 1 };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), EnteringStanding() });
		}

		[Fact]
		public void Next_SameEnteringState_KeepsPitStatus() {
			CarState ex = new CarState(key, EnteringStanding()) { LastPitLap = 2, LastPitTime = 157, PitThisLap = true, TotalPits = 1 };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), EnteringStanding(), EnteringStanding() });
		}

		[Fact]
		public void Next_NextLapInEnteringState_ResetsLapPitStatus() {
			CarState ex = new CarState(key, EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastPitLap = 2, LastPitTime = 157, StartedLapInPit = true, TotalPits = 1
			};
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_NextLapInExitingState_ResetsLapPitStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastPitLap = 2, LastPitTime = 157, PitThisLap = false, TotalPits = 1
			};
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_EnteringPitsOnConsecutiveLaps_SetsPitStatus() {
			CarState ex = new CarState(key, EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastPitLap = 3, LastPitTime = 220, PitThisLap = true, StartedLapInPit = true, TotalPits = 2
			};
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 10; s.sector = "SECTOR1"; }),
				BasicStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 30; s.sector = "SECTOR1"; }),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 60; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_OnTrackToStopped_SetsStopStatus() {
			CarState ex = new CarState(key, StoppedStanding()) { LastStopLap = 2, LastStopTime = 157, StopThisLap = true, TotalStops = 1 };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), StoppedStanding() });
		}

		[Fact]
		public void Next_SameStoppedState_KeepsStopStatus() {
			CarState ex = new CarState(key, StoppedStanding()) { LastStopLap = 2, LastStopTime = 157, StopThisLap = true, TotalStops = 1 };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), StoppedStanding(), StoppedStanding() });
		}

		[Fact]
		public void Next_NextLapInExitingState_ResetsLapStopStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 70; s.timeIntoLap = 0; s.sector = "SECTOR1"; })) {
				LastStopLap = 2, LastStopTime = 157, LastReleaseTime = 160, StopThisLap = false, TotalStops = 1
			};
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				StoppedStanding(),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 70; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_OnTrackPenalty_SetsPenalty() {
			CarState ex = new CarState(key, BasicStanding(s => s.penalties++)) { TotalPenalties = 1, PenaltyThisLap = true };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.penalties++) });
		}

		[Fact]
		public void Next_OnTrackPenaltyServed_KeepsPenalty() {
			CarState ex = new CarState(key, BasicStanding(s => s.lapsCompleted++)) { TotalPenalties = 1 };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.penalties++), BasicStanding(s => s.lapsCompleted++) });
		}

		[Fact]
		public void Next_OnTrackToGarage_SetsGarage() {
			CarState ex = new CarState(key, BasicStanding(s => s.inGarageStall = true)) { GarageThisLap = true };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.inGarageStall = true) });
		}

		[Fact]
		public void Next_GarageToOnTrack_KeepsGarage() {
			CarState ex = new CarState(key, BasicStanding()) { GarageThisLap = true };
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.inGarageStall = true), BasicStanding() });
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector1_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; })) {
				LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 1
			};
			AssertCarState(ex, new List<Standing>() { EnteringStanding(), ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; }) });
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector2_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; })) {
				LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 2
			};
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector3_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; })) {
				LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 3
			};
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapGarage_SetsSwapStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; })) {
				LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 0, GarageThisLap = true
			};
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapToOnTrack_KeepsSwapStatus() {
			CarState ex = new CarState(key, BasicStanding(s => s.driverName = "driver2")) {
				LastSwapLap = 2, LastSwapTime = 160, SwapThisLap = true, SwapLocation = 3
			};
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => s.driverName = "driver2"),
				BasicStanding(s => s.driverName = "driver2")
			});
		}

		[Fact]
		public void Next_PitStateNoneDuringDriverSwap_SetsStatus() {
			CarState ex = new CarState(key, ExitingStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; })) {
				LastStopLap = 2, LastStopTime = 160, StopThisLap = true, LastReleaseTime = 163, LastSwapLap = 2, LastSwapTime = 162, SwapThisLap = true, SwapLocation = 3, TotalStops = 1
			};
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(s => s.timeIntoLap = 50),
				StoppedStanding(s => s.timeIntoLap = 60),
				StoppedStanding(s => { s.timeIntoLap = 61; s.pitState = "NONE"; }),
				StoppedStanding(s => { s.timeIntoLap = 62; s.pitState = "STOPPED"; s.driverName = "driver2"; }),
				ExitingStanding(s => { s.timeIntoLap = 63; s.driverName = "driver2"; }),
				ExitingStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; })
			});
		}
	}
}
