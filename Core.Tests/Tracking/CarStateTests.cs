using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;
using static LMUSessionTracker.Core.LMU.PitState;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class CarStateTests {
		private readonly CarKey key = new CarKey() { SlotId = 0, Veh = "someveh" };

		public CarStateTests() {
		}

		private T Apply<T>(T t, Action<T> action) {
			if(action != null)
				action(t);
			return t;
		}

		private Standing Standing(Standing standing, Action<Standing> action) {
			return Apply(standing, action);
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
				pitState = NONE,
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
				pitState = ENTERING,
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
				pitState = STOPPED,
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
				pitState = EXITING,
				pitstops = 0,
				pitting = true,
				position = 1,
				serverScored = true,
				sector = "SECTOR3",
				timeIntoLap = 60,
			}, action);
		}

		private CarState From(Standing standing) {
			return new CarState(key) {
				CountLapFlag = standing.countLapFlag,
				DriverName = standing.driverName,
				FinishStatus = standing.finishStatus,
				InGarageStall = standing.inGarageStall,
				LapStartET = standing.lapStartET,
				LapsCompleted = standing.lapsCompleted,
				Penalties = standing.penalties,
				PitState = standing.pitState,
				Pitstops = standing.pitstops,
				Pitting = standing.pitting,
				Position = standing.position,
				ServerScored = standing.serverScored,
			};
		}

		private CarState CarState(Action<CarState> action = null) => CarState(null, action);

		private CarState CarState(Standing standing = null, Action<CarState> action = null) {
			CarState cs = standing == null ? new CarState(key) : new CarState(key, standing.lapStartET + standing.timeIntoLap, standing);
			return Apply(cs, action);
		}

		private void AssertCarState(CarState ex, List<Standing> standingSequence) {
			CarState ac = CarState(standingSequence[0]);
			// timeIntoLap stops increasing when in the pits for some reason, but it is used here for the tests
			for(int i = 1; i < standingSequence.Count; i++)
				ac = ac.Next(standingSequence[i].lapStartET + standingSequence[i].timeIntoLap, standingSequence[i]);
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void Construct_OnTrack_NoExtraStatusSet() {
			Assert.Equivalent(From(BasicStanding()), CarState(BasicStanding()));
		}

		[Fact]
		public void Construct_Entering_SetsPitStatus() {
			Assert.Equivalent(Apply(From(EnteringStanding()), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
			}), CarState(EnteringStanding()));
		}

		[Fact]
		public void Construct_Stopped_SetsPitAndStopStatus() {
			Assert.Equivalent(Apply(From(StoppedStanding()), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
				s.LastStopLap = 2; s.LastStopTime = 157; s.StopThisLap = true; s.StopLocation = 3; s.TotalStops = 1;
			}), CarState(StoppedStanding()));
		}

		[Fact]
		public void Construct_Garage_SetsGarageStatus() {
			Assert.Equivalent(Apply(From(BasicStanding(s => { s.inGarageStall = true; })), s => {
				s.LastGarageLap = 2; s.LastGarageInTime = 111; s.GarageThisLap = true;
			}), CarState(BasicStanding(s => { s.inGarageStall = true; })));
		}

		[Fact]
		public void Construct_Penalties_SetPenalty() {
			Assert.Equivalent(Apply(From(BasicStanding()), s => {
				s.Penalties = 2; s.PenaltyThisLap = true; s.TotalPenalties = 2;
			}), CarState(BasicStanding(s => { s.penalties = 2; })));
		}

		[Fact]
		public void Next_SameOnTrackState_NoChanges() {
			AssertCarState(CarState(BasicStanding()), new List<Standing>() { BasicStanding(), BasicStanding() });
		}

		[Fact]
		public void Next_OnTrackToEntering_SetsPitStatus() {
			CarState ex = CarState(EnteringStanding(), s => { s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), EnteringStanding() });
		}

		[Fact]
		public void Next_SameEnteringState_KeepsPitStatus() {
			CarState ex = CarState(EnteringStanding(), s => { s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), EnteringStanding(), EnteringStanding() });
		}

		[Fact]
		public void Next_NextLapInEnteringState_ResetsLapPitStatus() {
			CarState ex = Apply(From(EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })), s => {
				s.LastPitLap = 2; s.LastPitTime = 157;
				s.LastLapEndPitState = ENTERING; s.ThisLapStartPitState = ENTERING; s.TotalPits = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_NextLapInExitingState_ResetsLapPitStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = false;
				s.LastLapEndPitState = ENTERING; s.ThisLapStartPitState = EXITING; s.TotalPits = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_ExitingPits_SetsPitExitStatus() {
			CarState ex = CarState(BasicStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = false; s.LastExitTime = 190;
				s.LastLapEndPitState = ENTERING; s.ThisLapStartPitState = EXITING; s.TotalPits = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				BasicStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 30; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_ExitingPitsNextLapInExitingState_SetsPitExitStatus() {
			CarState ex = CarState(BasicStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = false; s.LastExitTime = 190;
				s.LastLapEndPitState = EXITING; s.ThisLapStartPitState = EXITING; s.TotalPits = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(),
				ExitingStanding(s => s.timeIntoLap = 59),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				BasicStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 30; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_EnteringPitsOnConsecutiveLaps_SetsPitStatus() {
			CarState ex = CarState(EnteringStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }), s => {
				s.LastPitLap = 3; s.LastPitTime = 220; s.PitThisLap = true; s.LastExitTime = 190;
				s.LastLapEndPitState = ENTERING; s.ThisLapStartPitState = ENTERING; s.TotalPits = 2;
;			});
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
			CarState ex = Apply(From(StoppedStanding()), s => {
				s.LastStopLap = 2; s.LastStopTime = 157; s.StopThisLap = true; s.StopLocation = 3; s.TotalStops = 1;
			});
			AssertCarState(ex, new List<Standing>() { BasicStanding(), StoppedStanding() });
		}

		[Fact]
		public void Next_SameStoppedState_KeepsStopStatus() {
			CarState ex = Apply(From(StoppedStanding()), s => {
				s.LastStopLap = 2; s.LastStopTime = 157; s.StopThisLap = true; s.StopLocation = 3; s.TotalStops = 1;
			});
			AssertCarState(ex, new List<Standing>() { BasicStanding(), StoppedStanding(), StoppedStanding() });
		}

		[Fact]
		public void Next_NextLapInExitingState_ResetsLapStopStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 70; s.timeIntoLap = 0; s.sector = "SECTOR1"; }), s => {
				s.LastStopLap = 2; s.LastStopTime = 157; s.LastReleaseTime = 160; s.StopThisLap = false; s.LastLapEndPitState = STOPPED; s.ThisLapStartPitState = EXITING; s.TotalStops = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				StoppedStanding(),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 60; s.timeIntoLap = 0; s.sector = "SECTOR1"; }),
				ExitingStanding(s => { s.lapsCompleted++; s.lapStartET += 70; s.timeIntoLap = 0; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_OnTrackPenalty_SetsPenalty() {
			CarState ex = CarState(BasicStanding(s => s.penalties++), s => { s.TotalPenalties = 1; s.PenaltyThisLap = true; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.penalties++) });
		}

		[Fact]
		public void Next_OnTrackPenaltyServed_KeepsPenalty() {
			CarState ex = CarState(BasicStanding(s => s.lapsCompleted++), s => { s.LastLapEndPitState = NONE; s.ThisLapStartPitState = NONE; s.TotalPenalties = 1; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => s.penalties++), BasicStanding(s => s.lapsCompleted++) });
		}

		[Fact]
		public void Next_OnTrackToGarage_SetsGarageIn() {
			CarState ex = CarState(BasicStanding(s => s.inGarageStall = true), s => { s.LastGarageLap = 2; s.LastGarageInTime = 112; s.GarageThisLap = true; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => { s.inGarageStall = true; s.timeIntoLap += 1; }) });
		}

		[Fact]
		public void Next_GarageToOnTrack_SetGarageOut() {
			CarState ex = CarState(BasicStanding(), s => { s.LastGarageLap = 2; s.LastGarageInTime = 112; s.LastGarageOutTime = 113; s.GarageThisLap = true; });
			AssertCarState(ex, new List<Standing>() { BasicStanding(), BasicStanding(s => { s.inGarageStall = true; s.timeIntoLap += 1; }), BasicStanding(s => s.timeIntoLap += 2) });
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector1_SetsSwapStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
				s.LastSwapLap = 2; s.LastSwapTime = 160; s.SwapThisLap = true; s.SwapLocation = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(), ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR1"; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector2_SetsSwapStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
				s.LastSwapLap = 2; s.LastSwapTime = 160; s.SwapThisLap = true; s.SwapLocation = 2;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR2"; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapSector3_SetsSwapStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
				s.LastSwapLap = 2; s.LastSwapTime = 160; s.SwapThisLap = true; s.SwapLocation = 3;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.sector = "SECTOR3"; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapGarage_SetsSwapStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.TotalPits = 1;
				s.LastSwapLap = 2; s.LastSwapTime = 160; s.SwapThisLap = true; s.SwapLocation = 0;
				s.LastGarageLap = 2; s.LastGarageInTime = 160; s.GarageThisLap = true;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => { s.driverName = "driver2"; s.inGarageStall = true; })
			});
		}

		[Fact]
		public void Next_EnteringToExitingDriverSwapToOnTrack_KeepsSwapStatus() {
			CarState ex = CarState(BasicStanding(s => s.driverName = "driver2"), s => {
				s.LastPitLap = 2; s.LastPitTime = 157; s.PitThisLap = true; s.LastExitTime = 170; s.TotalPits = 1;
				s.LastSwapLap = 2; s.LastSwapTime = 160; s.SwapThisLap = true; s.SwapLocation = 3;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(),
				ExitingStanding(s => s.driverName = "driver2"),
				BasicStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; })
			});
		}

		[Fact]
		public void Next_PitStateNoneDuringDriverSwap_SetsStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 150; s.PitThisLap = true; s.TotalPits = 1;
				s.LastStopLap = 2; s.LastStopTime = 160; s.StopThisLap = true; s.LastReleaseTime = 163; s.StopLocation = 3;
				s.LastSwapLap = 2; s.LastSwapTime = 162; s.SwapThisLap = true; s.SwapLocation = 3; s.TotalStops = 1;
;			});
			AssertCarState(ex, new List<Standing>() {
				EnteringStanding(s => s.timeIntoLap = 50),
				StoppedStanding(s => s.timeIntoLap = 60),
				StoppedStanding(s => { s.timeIntoLap = 61; s.pitState = NONE; }),
				StoppedStanding(s => { s.timeIntoLap = 62; s.pitState = STOPPED; s.driverName = "driver2"; }),
				ExitingStanding(s => { s.timeIntoLap = 63; s.driverName = "driver2"; }),
				ExitingStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; })
			});
		}

		[Fact]
		public void Next_MultiplePitsOnSameLap_SetsPitStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.timeIntoLap = 100; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 190; s.PitThisLap = true; s.LastExitTime = 180; s.TotalPits = 2;
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(s => { s.timeIntoLap = 60; }),
				ExitingStanding(s => { s.timeIntoLap = 70; }),
				BasicStanding(s => { s.timeIntoLap = 80; }),
				EnteringStanding(s => { s.timeIntoLap = 90; }),
				ExitingStanding(s => { s.timeIntoLap = 100; }),
			});
		}

		[Fact]
		public void Next_MultipleStopsOnSameLap_SetsPitStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.timeIntoLap = 100; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 190; s.PitThisLap = true;
				s.LastStopLap = 2; s.LastStopTime = 195; s.StopThisLap = true; s.LastReleaseTime = 200; s.StopLocation = 3;
				s.LastExitTime = 180; s.TotalPits = 2; s.TotalStops = 2
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(s => { s.timeIntoLap = 60; }),
				StoppedStanding(s => { s.timeIntoLap = 65; }),
				ExitingStanding(s => { s.timeIntoLap = 70; }),
				BasicStanding(s => { s.timeIntoLap = 80; }),
				EnteringStanding(s => { s.timeIntoLap = 90; }),
				StoppedStanding(s => { s.timeIntoLap = 95; }),
				ExitingStanding(s => { s.timeIntoLap = 100; }),
			});
		}

		[Fact]
		public void Next_MultipleSwapsOnSameLap_SetsPitStatus() {
			CarState ex = CarState(ExitingStanding(s => { s.timeIntoLap = 100; }), s => {
				s.LastPitLap = 2; s.LastPitTime = 190; s.PitThisLap = true;
				s.LastStopLap = 2; s.LastStopTime = 195; s.StopThisLap = true; s.LastReleaseTime = 200; s.StopLocation = 3;
				s.LastExitTime = 180; s.LastSwapLap = 2; s.LastSwapTime = 200; s.SwapThisLap = true; s.SwapLocation = 3; s.TotalPits = 2; s.TotalStops = 2
;			});
			AssertCarState(ex, new List<Standing>() {
				BasicStanding(),
				EnteringStanding(s => { s.timeIntoLap = 60; }),
				StoppedStanding(s => { s.timeIntoLap = 65; }),
				ExitingStanding(s => { s.timeIntoLap = 70; s.driverName = "driver2"; }),
				BasicStanding(s => { s.timeIntoLap = 80; s.driverName = "driver2"; }),
				EnteringStanding(s => { s.timeIntoLap = 90; s.driverName = "driver2"; }),
				StoppedStanding(s => { s.timeIntoLap = 95; s.driverName = "driver2"; }),
				ExitingStanding(s => { s.timeIntoLap = 100; }),
			});
		}

		[Fact]
		public void Difference() {
			CarState cs1 = new CarState(key) {
				CountLapFlag = "a",
				DriverName = "a",
				FinishStatus = "a",
				InGarageStall = false,
				LapStartET = 1.0,
				LapsCompleted = 1,
				Penalties = 1,
				PitState = "a",
				Pitstops = 1,
				Pitting = false,
				Position = 1,
				ServerScored = false,
				LastPitLap = 1,
				LastPitTime = 1.0,
				PitThisLap = false,
				LastStopLap = 1,
				LastStopTime = 1.0,
				StopThisLap = false,
				LastReleaseTime = 1.0,
				StopLocation = 1,
				LastExitTime = 1.0,
				LastGarageLap = 1,
				LastGarageInTime = 1.0,
				LastGarageOutTime = 1.0,
				GarageThisLap = false,
				LastSwapLap = 1,
				LastSwapTime = 1.0,
				SwapThisLap = false,
				SwapLocation = 1,
				LastLapEndPitState = "a",
				ThisLapStartPitState = "a",
				PenaltyThisLap = false,
				TotalPenalties = 1,
				TotalPits = 1,
				TotalStops = 1,
			};
			CarState cs2 = new CarState(key) {
				CountLapFlag = "b",
				DriverName = "b",
				FinishStatus = "b",
				InGarageStall = true,
				LapStartET = 2.0,
				LapsCompleted = 2,
				Penalties = 2,
				PitState = "b",
				Pitstops = 2,
				Pitting = true,
				Position = 2,
				ServerScored = true,
				LastPitLap = 2,
				LastPitTime = 2.0,
				PitThisLap = true,
				LastStopLap = 2,
				LastStopTime = 2.0,
				StopThisLap = true,
				LastReleaseTime = 2.0,
				StopLocation = 2,
				LastExitTime = 2.0,
				LastGarageLap = 2,
				LastGarageInTime = 2.0,
				LastGarageOutTime = 2.0,
				GarageThisLap = true,
				LastSwapLap = 2,
				LastSwapTime = 2.0,
				SwapThisLap = true,
				SwapLocation = 2,
				LastLapEndPitState = "b",
				ThisLapStartPitState = "b",
				PenaltyThisLap = true,
				TotalPenalties = 2,
				TotalPits = 2,
				TotalStops = 2,
			};
			Assert.Empty(cs1.Difference(cs1));
			Assert.Empty(cs2.Difference(cs2));
			Assert.Empty(cs1.Difference(cs1.Clone()));
			Assert.Empty(cs2.Difference(cs2.Clone()));
			Assert.Equal(34, cs1.Difference(cs2).Count);
		}
	}
}
