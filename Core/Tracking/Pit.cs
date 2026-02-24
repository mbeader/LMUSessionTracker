namespace LMUSessionTracker.Core.Tracking {
	public class Pit {
		public int Lap { get; set; }
		public double PitTime { get; set; } = -1;
		public double StopTime { get; set; } = -1;
		public double ExitTime { get; set; } = -1;
		public double SwapTime { get; set; } = -1;
		public bool StopAfterLine { get; set; }
		public bool Swap { get; set; }
		public int SwapLocation { get; set; } = -1;
		public bool Penalty { get; set; }
		public double Fuel { get; set; } = -1;
		public double VirtualEnergy { get; set; } = -1;
		public bool LFChanged { get; set; }
		public string LFCompound { get; set; }
		public bool LFNew { get; set; }
		public double LFUsage { get; set; } = -1;
		public bool RFChanged { get; set; }
		public string RFCompound { get; set; }
		public bool RFNew { get; set; }
		public double RFUsage { get; set; } = -1;
		public bool LRChanged { get; set; }
		public string LRCompound { get; set; }
		public bool LRNew { get; set; }
		public double LRUsage { get; set; } = -1;
		public bool RRChanged { get; set; }
		public string RRCompound { get; set; }
		public bool RRNew { get; set; }
		public double RRUsage { get; set; } = -1;
		public double PreviousStintDuration { get; set; } = -1;
		public double Time { get; set; } = -1;

		public Pit() { }

		public Pit(CarState state) {
			Lap = state.LapsCompleted + 1;
			Merge(state);
		}

		public void Merge(CarState state) {
			bool sameLap = Lap == state.LapsCompleted + 1;
			if(sameLap && state.PitThisLap)
				PitTime = state.LastPitTime;
			if((sameLap || state.StartedLapInPit) && state.StopThisLap) {
				if(state.LastStopTime > state.LastPitTime)
					StopTime = state.LastStopTime;
				if(state.LastExitTime > state.LastPitTime)
					ExitTime = state.LastExitTime;
			}
			if((sameLap || state.StartedLapInPit) && state.SwapThisLap && state.LastSwapTime > state.LastPitTime) {
				SwapTime = state.LastSwapTime;
				Swap = true;
				SwapLocation = state.SwapLocation;
			}
			StopAfterLine = state.StartedLapInPit;
		}

		public Pit Clone() {
			return new Pit() {
				Lap = Lap,
				PitTime = PitTime,
				StopTime = StopTime,
				ExitTime = ExitTime,
				SwapTime = SwapTime,
				StopAfterLine = StopAfterLine,
				Swap = Swap,
				SwapLocation = SwapLocation,
				Penalty = Penalty,
				Fuel = Fuel,
				VirtualEnergy = VirtualEnergy,
				LFChanged = LFChanged,
				LFCompound = LFCompound,
				LFNew = LFNew,
				LFUsage = LFUsage,
				RFChanged = RFChanged,
				RFCompound = RFCompound,
				RFNew = RFNew,
				RFUsage = RFUsage,
				LRChanged = LRChanged,
				LRCompound = LRCompound,
				LRNew = LRNew,
				LRUsage = LRUsage,
				RRChanged = RRChanged,
				RRCompound = RRCompound,
				RRNew = RRNew,
				RRUsage = RRUsage,
				PreviousStintDuration = PreviousStintDuration,
				Time = Time,
			};
		}
	}
}
