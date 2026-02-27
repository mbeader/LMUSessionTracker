using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using static LMUSessionTracker.Core.LMU.PitState;

namespace LMUSessionTracker.Core.Tracking {
	public class CarState {
		public CarKey Key { get; set; }
		public string CountLapFlag { get; set; }
		public string DriverName { get; set; }
		public string FinishStatus { get; set; }
		public bool InGarageStall { get; set; }
		public double LapStartET { get; set; }
		public int LapsCompleted { get; set; }
		public int Penalties { get; set; }
		public string PitState { get; set; }
		public int Pitstops { get; set; }
		public bool Pitting { get; set; }
		public int Position { get; set; }
		public bool ServerScored { get; set; }

		/// <summary>
		/// Lap of last entrance into pit lane
		/// </summary>
		public int LastPitLap { get; set; } = -1;
		/// <summary>
		/// Elapsed time of last entrance into pit lane
		/// </summary>
		public double LastPitTime { get; set; } = -1;
		/// <summary>
		/// Whether pit lane was entered this lap (PitState set to ENTERING)
		/// </summary>
		public bool PitThisLap { get; set; } = false;
		/// <summary>
		/// Lap of last pit stop
		/// </summary>
		public int LastStopLap { get; set; } = -1;
		/// <summary>
		/// Elapsed time of start of last pit stop
		/// </summary>
		public double LastStopTime { get; set; } = -1;
		/// <summary>
		/// Whether a pit stop was made this lap (PitState set to STOPPED)
		/// </summary>
		public bool StopThisLap { get; set; } = false;
		/// <summary>
		/// Elapsed time of end of last pit stop (PitState changed from STOPPED to anything else)
		/// </summary>
		public double LastReleaseTime { get; set; } = -1;
		/// <summary>
		/// Sector where driver swap occurred
		/// </summary>
		public int StopLocation { get; set; } = -1;
		/// <summary>
		/// Elapsed time of last exit from pit lane (PitState changed from EXITING to anything while in the pits)
		/// </summary>
		public double LastExitTime { get; set; } = -1;
		/// <summary>
		/// Lap of last visit to garage
		/// </summary>
		public int LastGarageLap { get; set; } = -1;
		/// <summary>
		/// Elapsed time of last entrance to garage
		/// </summary>
		public double LastGarageInTime { get; set; } = -1;
		/// <summary>
		/// Elapsed time of last exit from garage
		/// </summary>
		public double LastGarageOutTime { get; set; } = -1;
		/// <summary>
		/// Whether in garage at all during this lap (InGarageStall ever set)
		/// </summary>
		public bool GarageThisLap { get; set; } = false;
		/// <summary>
		/// Lap of last driver swap
		/// </summary>
		public int LastSwapLap { get; set; } = -1;
		/// <summary>
		/// Elapsed time of last driver swap
		/// </summary>
		public double LastSwapTime { get; set; } = -1;
		/// <summary>
		/// Whether a driver swap occurred this lap (driver name change, either in pit or garage)
		/// </summary>
		public bool SwapThisLap { get; set; } = false;
		/// <summary>
		/// Sector where driver swap occurred (garage = 0)
		/// </summary>
		public int SwapLocation { get; set; } = -1;
		/// <summary>
		/// Last recorded PitState of last lap
		/// </summary>
		public string LastLapEndPitState { get; set; }
		/// <summary>
		/// First recorded PitState of this lap
		/// </summary>
		public string ThisLapStartPitState { get; set; }
		/// <summary>
		/// Whether a penalty was given this lap (Penalties increased)
		/// </summary>
		public bool PenaltyThisLap { get; set; } = false;
		/// <summary>
		/// Total penalties accrued in session (Penalties is penalties waiting to be served)
		/// </summary>
		public int TotalPenalties { get; set; }
		/// <summary>
		/// Total times pits were entered during this session
		/// </summary>
		public int TotalPits { get; set; }
		/// <summary>
		/// Total times pit stops were made during this session
		/// </summary>
		public int TotalStops { get; set; }

		/// <summary>
		/// Whether this lap began on pit lane before any pit stop
		/// </summary>
		public bool StartedLapInPit => LastLapEndPitState == ENTERING || LastLapEndPitState == STOPPED;

		public CarState(CarKey key) {
			Key = key;
		}

		public CarState(CarKey key, double currentET, Standing standing) : this(key) {
			if(standing != null) {
				From(standing);
				Init(currentET, standing.sector);
			}
		}

		private void Init(double currentET, string sector) {
			if(PitState == ENTERING || PitState == STOPPED) {
				LastPitLap = LapsCompleted + 1;
				LastPitTime = currentET;
				PitThisLap = true;
				TotalPits++;
				if(PitState == STOPPED) {
					LastStopLap = LapsCompleted + 1;
					LastStopTime = currentET;
					StopThisLap = true;
					StopLocation = Sector(sector);
					TotalStops++;
				}
			}

			if(Penalties > 0) {
				TotalPenalties = Penalties;
				PenaltyThisLap = true;
			}

			if(InGarageStall) {
				LastGarageLap = LapsCompleted + 1;
				LastGarageInTime = currentET;
				GarageThisLap = true;
			}
		}

		public CarState Next(double currentET, Standing standing) {
			CarState newState = new CarState(Key);
			newState.From(standing);

			newState.LastPitLap = LastPitLap;
			newState.LastPitTime = LastPitTime;
			newState.LastStopLap = LastStopLap;
			newState.LastStopTime = LastStopTime;
			newState.LastReleaseTime = LastReleaseTime;
			newState.LastExitTime = LastExitTime;
			newState.LastGarageLap = LastGarageLap;
			newState.LastGarageInTime = LastGarageInTime;
			newState.LastGarageOutTime = LastGarageOutTime;
			newState.LastSwapLap = LastSwapLap;
			newState.LastSwapTime = LastSwapTime;
			newState.TotalPenalties = TotalPenalties;
			newState.TotalPits = TotalPits;
			newState.TotalStops = TotalStops;

			if(LapsCompleted == newState.LapsCompleted) {
				newState.PitThisLap = PitThisLap;
				newState.StopThisLap = StopThisLap;
				newState.StopLocation = StopLocation;
				newState.GarageThisLap = GarageThisLap;
				newState.SwapThisLap = SwapThisLap;
				newState.SwapLocation = SwapLocation;
				newState.LastLapEndPitState = LastLapEndPitState;
				newState.ThisLapStartPitState = ThisLapStartPitState;
				newState.PenaltyThisLap = PenaltyThisLap;

				if(newState.PitThisLap && newState.PitState == ENTERING && PitState != ENTERING) {
					// already pit this lap
					newState.LastPitTime = currentET;
					newState.TotalPits++;
				} else if((!newState.StartedLapInPit || PitState != ENTERING) && !newState.PitThisLap && newState.PitState == ENTERING) {
					newState.LastPitLap = newState.LapsCompleted + 1;
					newState.LastPitTime = currentET;
					newState.PitThisLap = true;
					newState.TotalPits++;
				}
			} else {
				newState.LastLapEndPitState = PitState;
				newState.ThisLapStartPitState = newState.PitState;
			}

			if(!StopThisLap && newState.PitState == STOPPED) {
				newState.LastStopLap = newState.LapsCompleted + 1;
				newState.LastStopTime = currentET;
				newState.StopThisLap = true;
				newState.StopLocation = Sector(standing.sector);
				newState.TotalStops++;
				newState.LastReleaseTime = -1;
			} else if(StopThisLap && newState.PitState == STOPPED && newState.LastPitTime > newState.LastStopTime) {
				// already stopped this lap
				newState.LastStopTime = currentET;
				newState.StopLocation = Sector(standing.sector);
				newState.TotalStops++;
				newState.LastReleaseTime = -1;
			} else if(StopThisLap && PitState == STOPPED && newState.PitState != STOPPED && newState.LastReleaseTime == -1) {
				newState.LastReleaseTime = currentET;
			} else if(StopThisLap && PitState != STOPPED && newState.PitState == STOPPED && newState.LastReleaseTime != -1) {
				// PitState seems to become NONE during driver swaps
				newState.LastReleaseTime = -1;
			}

			if((PitThisLap || StartedLapInPit || ThisLapStartPitState == EXITING) && PitState == EXITING && newState.PitState != EXITING) {
				newState.LastExitTime = currentET;
			}

			// probably will miss consecutive drive-through penalties
			if(Penalties < newState.Penalties) {
				newState.TotalPenalties++;
				newState.PenaltyThisLap = true;
			}

			if(!InGarageStall && newState.InGarageStall) {
				newState.LastGarageLap = newState.LapsCompleted + 1;
				newState.LastGarageInTime = currentET;
				newState.GarageThisLap = true;
			} else if(InGarageStall && !newState.InGarageStall) {
				newState.LastGarageOutTime = currentET;
			}

			if(DriverName != newState.DriverName) {
				newState.LastSwapLap = newState.LapsCompleted + 1;
				newState.LastSwapTime = currentET;
				newState.SwapThisLap = true;
				if(newState.InGarageStall)
					newState.SwapLocation = 0;
				else {
					newState.SwapLocation = Sector(standing.sector);
				}
			}
			return newState;
		}

		private int Sector(string sector) {
			switch(sector) {
				case "SECTOR1":
					return 1;
				case "SECTOR2":
					return 2;
				case "SECTOR3":
					return 3;
				default:
					return -1;
			}
		}

		private void From(Standing standing) {
			CountLapFlag = standing.countLapFlag;
			DriverName = standing.driverName;
			FinishStatus = standing.finishStatus;
			InGarageStall = standing.inGarageStall;
			LapStartET = standing.lapStartET;
			LapsCompleted = standing.lapsCompleted;
			Penalties = standing.penalties;
			PitState = standing.pitState;
			Pitstops = standing.pitstops;
			Pitting = standing.pitting;
			Position = standing.position;
			ServerScored = standing.serverScored;
		}

		public CarState Clone() {
			return new CarState(Key) {
				CountLapFlag = CountLapFlag,
				DriverName = DriverName,
				FinishStatus = FinishStatus,
				InGarageStall = InGarageStall,
				LapStartET = LapStartET,
				LapsCompleted = LapsCompleted,
				Penalties = Penalties,
				PitState = PitState,
				Pitstops = Pitstops,
				Pitting = Pitting,
				Position = Position,
				ServerScored = ServerScored,

				LastPitLap = LastPitLap,
				LastPitTime = LastPitTime,
				PitThisLap = PitThisLap,
				LastStopLap = LastStopLap,
				LastStopTime = LastStopTime,
				StopThisLap = StopThisLap,
				LastReleaseTime = LastReleaseTime,
				StopLocation = StopLocation,
				LastExitTime = LastExitTime,
				LastGarageLap = LastGarageLap,
				LastGarageInTime = LastGarageInTime,
				LastGarageOutTime = LastGarageOutTime,
				GarageThisLap = GarageThisLap,
				LastSwapLap = LastSwapLap,
				LastSwapTime = LastSwapTime,
				SwapThisLap = SwapThisLap,
				SwapLocation = SwapLocation,
				LastLapEndPitState = LastLapEndPitState,
				ThisLapStartPitState = ThisLapStartPitState,
				PenaltyThisLap = PenaltyThisLap,
				TotalPenalties = TotalPenalties,
				TotalPits = TotalPits,
				TotalStops = TotalStops,
			};
		}

		public List<string> Difference(CarState other) {
			List<string> diffs = new List<string>();
			foreach((string name, Func<CarState, object> prop) in DiffProps.Props) {
				var thisProp = prop(this);
				var otherProp = prop(other);
				if((thisProp == null && otherProp != null) || (thisProp != null && !thisProp.Equals(otherProp)))
					diffs.Add($"{name}: [{prop(this)} to {prop(other)}]");
			}
			return diffs;
		}

		private static class DiffProps {
			private static readonly List<(string name, Func<CarState, object> prop)> props = new List<(string name, Func<CarState, object> prop)>() {
				(nameof(CountLapFlag), x => x.CountLapFlag),
				(nameof(DriverName), x => x.DriverName),
				(nameof(FinishStatus), x => x.FinishStatus),
				(nameof(InGarageStall), x => x.InGarageStall),
				(nameof(LapStartET), x => x.LapStartET),
				(nameof(LapsCompleted), x => x.LapsCompleted),
				(nameof(Penalties), x => x.Penalties),
				(nameof(PitState), x => x.PitState),
				(nameof(Pitstops), x => x.Pitstops),
				(nameof(Pitting), x => x.Pitting),
				//(nameof(Position), x => x.Position),
				(nameof(ServerScored), x => x.ServerScored),
				(nameof(LastPitLap), x => x.LastPitLap),
				(nameof(LastPitTime), x => x.LastPitTime),
				(nameof(PitThisLap), x => x.PitThisLap),
				(nameof(LastStopLap), x => x.LastStopLap),
				(nameof(LastStopTime), x => x.LastStopTime),
				(nameof(StopThisLap), x => x.StopThisLap),
				(nameof(LastReleaseTime), x => x.LastReleaseTime),
				(nameof(StopLocation), x => x.StopLocation),
				(nameof(LastExitTime), x => x.LastExitTime),
				(nameof(LastGarageLap), x => x.LastGarageLap),
				(nameof(LastGarageInTime), x => x.LastGarageInTime),
				(nameof(LastGarageOutTime), x => x.LastGarageOutTime),
				(nameof(GarageThisLap), x => x.GarageThisLap),
				(nameof(LastSwapLap), x => x.LastSwapLap),
				(nameof(LastSwapTime), x => x.LastSwapTime),
				(nameof(SwapThisLap), x => x.SwapThisLap),
				(nameof(SwapLocation), x => x.SwapLocation),
				(nameof(LastLapEndPitState), x => x.LastLapEndPitState),
				(nameof(ThisLapStartPitState), x => x.ThisLapStartPitState),
				(nameof(PenaltyThisLap), x => x.PenaltyThisLap),
				(nameof(TotalPenalties), x => x.TotalPenalties),
				(nameof(TotalPits), x => x.TotalPits),
				(nameof(TotalStops), x => x.TotalStops),
			};

			public static IReadOnlyList<(string name, Func<CarState, object> prop)> Props => props;
		}
	}
}
