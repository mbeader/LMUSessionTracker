using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public class CarState {
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
		public bool ServerScored { get; set; }

		public static CarState From(Standing standing) {
			CarState state = new CarState();
			state.CountLapFlag = standing.countLapFlag;
			state.DriverName = standing.driverName;
			state.FinishStatus = standing.finishStatus;
			state.InGarageStall = standing.inGarageStall;
			state.LapStartET = standing.lapStartET;
			state.LapsCompleted = standing.lapsCompleted;
			state.Penalties = standing.penalties;
			state.PitState = standing.pitState;
			state.Pitstops = standing.pitstops;
			state.Pitting = standing.pitting;
			state.ServerScored = standing.serverScored;
			return state;
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
			if(ServerScored != other.ServerScored)
				diffs.Add($"ServerScored: [{ServerScored} to {other.ServerScored}]");
			return diffs;
		}
	}
}
