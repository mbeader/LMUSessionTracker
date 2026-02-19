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

		public int LastPitLap { get; set; } = -1;
		public double LastPitTime { get; set; } = -1;
		public bool PitThisLap { get; set; } = false;
		public bool GarageThisLap { get; set; } = false;
		public int LastSwapLap { get; set; } = -1;
		public double LastSwapTime { get; set; } = -1;
		public bool SwapThisLap { get; set; } = false;
		public int SwapLocation { get; set; } = -1;
		public int TotalPenalties { get; set; }
		public int TotalPitstops { get; set; }

		public CarState(CarKey key, Standing standing = null) {
			Key = key;
			if(standing != null)
				From(standing);
		}

		public CarState Next(Standing standing) {
			CarState newState = new CarState(Key, standing);

			newState.LastPitLap = LastPitLap;
			newState.LastPitTime = LastPitTime;
			newState.LastSwapLap = LastSwapLap;
			newState.LastSwapTime = LastSwapTime;
			newState.TotalPenalties = TotalPenalties;
			newState.TotalPitstops = TotalPitstops;
			if(LapsCompleted == newState.LapsCompleted) {
				newState.PitThisLap = PitThisLap;
				newState.GarageThisLap = GarageThisLap;
				newState.SwapThisLap = SwapThisLap;
				newState.SwapLocation = SwapLocation;
			}

			if(!newState.PitThisLap && newState.PitState == "ENTERING" && !(PitThisLap && PitState == "ENTERING" && LapsCompleted < newState.LapsCompleted)) {
				newState.LastPitLap = newState.LapsCompleted + 1;
				if(standing.timeIntoLap >= 0)
					newState.LastPitTime = (newState.LapStartET < 0 ? 0 : newState.LapStartET) + standing.timeIntoLap;
				newState.PitThisLap = true;
				newState.TotalPitstops++;
			}

			// probably will miss consecutive drive-through penalties
			if(Penalties < newState.Penalties)
				newState.TotalPenalties++;

			if(newState.InGarageStall)
				newState.GarageThisLap = true;

			if(DriverName != newState.DriverName) {
				newState.LastSwapLap = newState.LapsCompleted + 1;
				if(standing.timeIntoLap >= 0)
					newState.LastSwapTime = (newState.LapStartET < 0 ? 0 : newState.LapStartET) + standing.timeIntoLap;
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
				LastSwapLap = LastSwapLap,
				LastSwapTime = LastSwapTime,
				SwapThisLap = SwapThisLap,
				SwapLocation = SwapLocation,
				TotalPenalties = TotalPenalties,
				TotalPitstops = TotalPitstops,
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
			if(LastSwapLap != other.LastSwapLap)
				diffs.Add($"LastSwapLap: [{LastSwapLap} to {other.LastSwapLap}]");
			if(LastSwapTime != other.LastSwapTime)
				diffs.Add($"LastSwapTime: [{LastSwapTime} to {other.LastSwapTime}]");
			if(SwapThisLap != other.SwapThisLap)
				diffs.Add($"SwapThisLap: [{SwapThisLap} to {other.SwapThisLap}]");
			if(SwapLocation != other.SwapLocation)
				diffs.Add($"SwapLocation: [{SwapLocation} to {other.SwapLocation}]");
			if(TotalPenalties != other.TotalPenalties)
				diffs.Add($"TotalPenalties: [{TotalPenalties} to {other.TotalPenalties}]");
			if(TotalPitstops != other.TotalPitstops)
				diffs.Add($"TotalPitstops: [{TotalPitstops} to {other.TotalPitstops}]");
			return diffs;
		}
	}
}
