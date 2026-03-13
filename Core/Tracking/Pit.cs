using LMUSessionTracker.Common.LMU;
using System;

namespace LMUSessionTracker.Core.Tracking {
	public class Pit {
		public int Lap { get; set; }
		public double PitTime { get; set; } = -1;
		public double StopTime { get; set; } = -1;
		public double ReleaseTime { get; set; } = -1;
		public double ExitTime { get; set; } = -1;
		public double GarageInTime { get; set; } = -1;
		public double GarageOutTime { get; set; } = -1;
		public double SwapTime { get; set; } = -1;
		public bool StopAfterLine { get; set; }
		public int StopLocation { get; set; } = -1;
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
		public bool Resolved { get; set; }

		private bool IsGarage => GarageInTime >= 0 || GarageOutTime >= 0;

		public Pit() { }

		public Pit(CarState state) {
			Lap = state.LapsCompleted + 1;
			Merge(state);
		}

		public void Merge(CarState state) {
			bool sameLap = Lap == state.LapsCompleted + 1;

			if(state.GarageThisLap) {
				double maxGarageTime = Math.Max(state.LastGarageInTime, state.LastGarageOutTime);
				if(!(state.LastPitTime > maxGarageTime || state.LastStopTime > maxGarageTime)) {
					GarageInTime = state.LastGarageInTime;
					if(state.LastGarageOutTime > state.LastGarageInTime)
						GarageOutTime = state.LastGarageOutTime;
					if(state.SwapThisLap && state.LastSwapTime > state.LastGarageInTime && (state.LastGarageOutTime < 0 || state.LastSwapTime < state.LastGarageOutTime)) {
						SwapTime = state.LastSwapTime;
						Swap = true;
						SwapLocation = state.SwapLocation;
					}
				}
			}
			if(IsGarage)
				return;

			if(sameLap && state.PitThisLap)
				PitTime = state.LastPitTime;
			if((sameLap || state.StartedLapInPit) && state.StopThisLap) {
				if(state.LastStopTime > state.LastPitTime) {
					StopTime = state.LastStopTime;
					StopLocation = state.StopLocation;
				}
				if(state.LastReleaseTime > state.LastPitTime)
					ReleaseTime = state.LastReleaseTime;
			}
			if(state.StartedLapInPit && !state.StopThisLap && state.LastLapEndPitState == PitState.STOPPED) {
				if(state.LastReleaseTime > state.LastStopTime)
					ReleaseTime = state.LastReleaseTime;
			}
			if((sameLap || state.StartedLapInPit) && state.SwapThisLap && state.LastSwapTime > state.LastPitTime) {
				SwapTime = state.LastSwapTime;
				Swap = true;
				SwapLocation = state.SwapLocation;
			}
			if((sameLap || state.StartedLapInPit || state.ThisLapStartPitState == PitState.EXITING) && state.LastExitTime > state.LastPitTime) {
				ExitTime = state.LastExitTime;
			}
			StopAfterLine = state.StartedLapInPit && (state.ThisLapStartPitState == PitState.STOPPED || state.LastLapEndPitState != PitState.STOPPED);
		}

		public void Resolve(Strategy strategy) {
			if(Resolved || IsDefaultStrategy(strategy))
				return;
			Penalty = strategy.penalty;
			Fuel = strategy.fuel ?? Fuel;
			VirtualEnergy = strategy.ve;
			if(strategy.tyres?.fl != null) {
				LFChanged = strategy.tyres.fl.changed;
				LFCompound = strategy.tyres.fl.compound;
				LFNew = strategy.tyres.fl.New;
				LFUsage = strategy.tyres.fl.usage ?? LFUsage;
			}
			if(strategy.tyres?.fr != null) {
				RFChanged = strategy.tyres.fr.changed;
				RFCompound = strategy.tyres.fr.compound;
				RFNew = strategy.tyres.fr.New;
				RFUsage = strategy.tyres.fr.usage ?? RFUsage;
			}
			if(strategy.tyres?.rl != null) {
				LRChanged = strategy.tyres.rl.changed;
				LRCompound = strategy.tyres.rl.compound;
				LRNew = strategy.tyres.rl.New;
				LRUsage = strategy.tyres.rl.usage ?? LRUsage;
			}
			if(strategy.tyres?.rr != null) {
				RRChanged = strategy.tyres.rr.changed;
				RRCompound = strategy.tyres.rr.compound;
				RRNew = strategy.tyres.rr.New;
				RRUsage = strategy.tyres.rr.usage ?? RRUsage;
			}
			PreviousStintDuration = strategy.previousStintDuration;
			Time = strategy.time;
			Resolved = true;
		}

		public static bool IsDefaultStrategy(Strategy strategy) {
			StrategyTire tire = strategy?.tyres?.fl;
			return tire?.compound == null || tire.compound == "N/A";
			//return !strategy.driverSwap &&
			//	!strategy.penalty &&
			//	strategy.previousStintDuration == 0.0 &&
			//	strategy.time == 0.0 &&
			//	strategy.ve == 1.0 &&
			//	IsDefaultTire(strategy.tyres?.fl) &&
			//	IsDefaultTire(strategy.tyres?.fr) &&
			//	IsDefaultTire(strategy.tyres?.rl) &&
			//	IsDefaultTire(strategy.tyres?.rr);
		}

		private static bool IsDefaultTire(StrategyTire tire) {
			return tire == null || (!tire.changed && tire.New && (tire.compound == null || tire.compound == "N/A"));
		}

		public Pit Clone() {
			return new Pit() {
				Lap = Lap,
				PitTime = PitTime,
				StopTime = StopTime,
				ReleaseTime = ReleaseTime,
				ExitTime = ExitTime,
				GarageInTime = GarageInTime,
				GarageOutTime = GarageOutTime,
				SwapTime = SwapTime,
				StopAfterLine = StopAfterLine,
				StopLocation = StopLocation,
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
				Resolved = Resolved
			};
		}
	}
}
