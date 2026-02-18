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

		public CarState(CarKey key, Standing standing = null) {
			Key = key;
			if(standing != null)
				From(standing);
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
			return diffs;
		}
	}
}
