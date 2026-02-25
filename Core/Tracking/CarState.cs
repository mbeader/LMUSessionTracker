using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

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
		/// Whether this lap began on pit lane before any pit stop (PitState set to ENTERING during lap change)
		/// </summary>
		public bool StartedLapInPit { get; set; } = false;
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

		public CarState(CarKey key, Standing standing = null) {
			Key = key;
			if(standing != null)
				From(standing);
		}

		public CarState Next(double currentET, Standing standing) {
			CarState newState = new CarState(Key, standing);

			newState.LastPitLap = LastPitLap;
			newState.LastPitTime = LastPitTime;
			newState.LastStopLap = LastStopLap;
			newState.LastStopTime = LastStopTime;
			newState.LastReleaseTime = LastReleaseTime;
			newState.LastSwapLap = LastSwapLap;
			newState.LastSwapTime = LastSwapTime;
			newState.TotalPenalties = TotalPenalties;
			newState.TotalPits = TotalPits;
			newState.TotalStops = TotalStops;

			if(LapsCompleted == newState.LapsCompleted) {
				newState.PitThisLap = PitThisLap;
				newState.StopThisLap = StopThisLap;
				newState.GarageThisLap = GarageThisLap;
				newState.SwapThisLap = SwapThisLap;
				newState.SwapLocation = SwapLocation;
				newState.StartedLapInPit = StartedLapInPit;
				newState.PenaltyThisLap = PenaltyThisLap;

				if((!newState.StartedLapInPit || PitState != "ENTERING") && !newState.PitThisLap && newState.PitState == "ENTERING") {
					newState.LastPitLap = newState.LapsCompleted + 1;
					newState.LastPitTime = currentET;
					newState.PitThisLap = true;
					newState.TotalPits++;
				}
			} else {
				if(newState.PitState == "ENTERING")
					newState.StartedLapInPit = true;
			}

			if(!StopThisLap && newState.PitState == "STOPPED") {
				newState.LastStopLap = newState.LapsCompleted + 1;
				newState.LastStopTime = currentET;
				newState.StopThisLap = true;
				newState.TotalStops++;
				newState.LastReleaseTime = -1;
			} else if(StopThisLap && PitState == "STOPPED" && newState.PitState != "STOPPED" && newState.LastReleaseTime == -1) {
				newState.LastReleaseTime = currentET;
			} else if(StopThisLap && PitState != "STOPPED" && newState.PitState == "STOPPED" && newState.LastReleaseTime != -1) {
				// PitState seems to become NONE during driver swaps
				newState.LastReleaseTime = -1;
			}

			// probably will miss consecutive drive-through penalties
			if(Penalties < newState.Penalties) {
				newState.TotalPenalties++;
				newState.PenaltyThisLap = true;
			}

			if(newState.InGarageStall)
				newState.GarageThisLap = true;

			if(DriverName != newState.DriverName) {
				newState.LastSwapLap = newState.LapsCompleted + 1;
				newState.LastSwapTime = currentET;
				newState.SwapThisLap = true;
				if(newState.InGarageStall)
					newState.SwapLocation = 0;
				else {
					switch(standing.sector) {
						case "SECTOR1":
							newState.SwapLocation = 1;
							break;
						case "SECTOR2":
							newState.SwapLocation = 2;
							break;
						case "SECTOR3":
							newState.SwapLocation = 3;
							break;
					}
				}
			}
			return newState;
		}

		public void From(Standing standing) {
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
				GarageThisLap = GarageThisLap,
				LastSwapLap = LastSwapLap,
				LastSwapTime = LastSwapTime,
				SwapThisLap = SwapThisLap,
				SwapLocation = SwapLocation,
				StartedLapInPit = StartedLapInPit,
				PenaltyThisLap = PenaltyThisLap,
				TotalPenalties = TotalPenalties,
				TotalPits = TotalPits,
				TotalStops = TotalStops,
			};
		}

		public List<string> Difference(CarState other) {
			List<string> diffs = new List<string>();
			if(CountLapFlag != other.CountLapFlag)
				diffs.Add($"CountLapFlag: [{CountLapFlag} to {other.CountLapFlag}]");
			if(DriverName != other.DriverName)
				diffs.Add($"DriverName: [{DriverName} to {other.DriverName}]");
			if(FinishStatus != other.FinishStatus)
				diffs.Add($"FinishStatus: [{FinishStatus} to {other.FinishStatus}]");
			if(InGarageStall != other.InGarageStall)
				diffs.Add($"InGarageStall: [{InGarageStall} to {other.InGarageStall}]");
			if(LapStartET != other.LapStartET)
				diffs.Add($"LapStartET: [{LapStartET} to {other.LapStartET}]");
			if(LapsCompleted != other.LapsCompleted)
				diffs.Add($"LapsCompleted: [{LapsCompleted} to {other.LapsCompleted}]");
			if(Penalties != other.Penalties)
				diffs.Add($"Penalties: [{Penalties} to {other.Penalties}]");
			if(PitState != other.PitState)
				diffs.Add($"PitState: [{PitState} to {other.PitState}]");
			if(Pitstops != other.Pitstops)
				diffs.Add($"Pitstops: [{Pitstops} to {other.Pitstops}]");
			if(Pitting != other.Pitting)
				diffs.Add($"Pitting: [{Pitting} to {other.Pitting}]");
			//if(Position != other.Position)
			//	diffs.Add($"Position: [{Position} to {other.Position}]");
			if(ServerScored != other.ServerScored)
				diffs.Add($"ServerScored: [{ServerScored} to {other.ServerScored}]");
			if(LastPitLap != other.LastPitLap)
				diffs.Add($"LastPitLap: [{LastPitLap} to {other.LastPitLap}]");
			if(LastPitTime != other.LastPitTime)
				diffs.Add($"LastPitTime: [{LastPitTime} to {other.LastPitTime}]");
			if(PitThisLap != other.PitThisLap)
				diffs.Add($"PitThisLap: [{PitThisLap} to {other.PitThisLap}]");
			if(LastStopLap != other.LastStopLap)
				diffs.Add($"LastStopLap: [{LastStopLap} to {other.LastStopLap}]");
			if(LastStopTime != other.LastStopTime)
				diffs.Add($"LastStopTime: [{LastStopTime} to {other.LastStopTime}]");
			if(StopThisLap != other.StopThisLap)
				diffs.Add($"StopThisLap: [{StopThisLap} to {other.StopThisLap}]");
			if(LastReleaseTime != other.LastReleaseTime)
				diffs.Add($"LastReleaseTime: [{LastReleaseTime} to {other.LastReleaseTime}]");
			if(GarageThisLap != other.GarageThisLap)
				diffs.Add($"GarageThisLap: [{GarageThisLap} to {other.GarageThisLap}]");
			if(LastSwapLap != other.LastSwapLap)
				diffs.Add($"LastSwapLap: [{LastSwapLap} to {other.LastSwapLap}]");
			if(LastSwapTime != other.LastSwapTime)
				diffs.Add($"LastSwapTime: [{LastSwapTime} to {other.LastSwapTime}]");
			if(SwapThisLap != other.SwapThisLap)
				diffs.Add($"SwapThisLap: [{SwapThisLap} to {other.SwapThisLap}]");
			if(SwapLocation != other.SwapLocation)
				diffs.Add($"SwapLocation: [{SwapLocation} to {other.SwapLocation}]");
			if(StartedLapInPit != other.StartedLapInPit)
				diffs.Add($"StartedLapInPit: [{StartedLapInPit} to {other.StartedLapInPit}]");
			if(PenaltyThisLap != other.PenaltyThisLap)
				diffs.Add($"PenaltyThisLap: [{PenaltyThisLap} to {other.PenaltyThisLap}]");
			if(TotalPenalties != other.TotalPenalties)
				diffs.Add($"TotalPenalties: [{TotalPenalties} to {other.TotalPenalties}]");
			if(TotalPits != other.TotalPits)
				diffs.Add($"TotalPits: [{TotalPits} to {other.TotalPits}]");
			if(TotalStops != other.TotalStops)
				diffs.Add($"TotalStops: [{TotalStops} to {other.TotalStops}]");
			return diffs;
		}
	}
}
