using LMUSessionTracker.Common.LMU;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarHistory {
		public CarKey Key { get; private set; }
		public Car Car { get; private set; }
		public List<Lap> Laps { get; } = new List<Lap>();
		public List<Pit> Pits { get; } = new List<Pit>();
		public int LapsCompleted { get; private set; }

		public CarHistory(CarKey key, Car car, List<Lap> laps = null, List<Pit> pits = null) {
			Key = key;
			Car = car;
			if(laps != null) {
				Laps.AddRange(laps);
				LapsCompleted = Laps.Count;
			}
			if(pits != null) {
				Pits.AddRange(pits);
			}
		}

		private Lap AddLap(Lap lap, DateTime timestamp) {
			while(lap.LapNumber > Laps.Count)
				Laps.Add(null);
			bool added = false;
			if(Laps[lap.LapNumber - 1] == null) {
				Laps[lap.LapNumber - 1] = lap;
				lap.Timestamp = timestamp;
				added = true;
			}
			LapsCompleted = Laps.Count;
			return added ? lap : null;
		}

		public Lap GetLap(int lapNumber) {
			if(lapNumber > Laps.Count)
				return null;
			return Laps[lapNumber - 1] ?? Lap.Default(lapNumber);
		}

		public Lap Update(UpdateContext<CarHistory> context, CarStateChange state, Standing standing, WSStandingSubset wsStanding, List<Strategy> strategy, List<StrategyDriverUsage> usage) {
			Lap newLap = null;
			if(standing.lapsCompleted > 0)
				newLap = AddLap(new Lap(standing, wsStanding, state.Previous), context.Timestamp);
			AddPit(state, newLap);
			AddStrategy(context, strategy);
			AddUsage(context, usage);
			if(!Car.HasAllFields)
				Car.Merge(new Car(standing));
			if(context.Options.TraceLogging && newLap != null && !newLap.Resolved)
				context.Logger.LogInformation($"{Key.Id()} unresolved [{newLap.LapNumber}]");
			return newLap;
		}

		private void AddPit(CarStateChange state, Lap newLap) {
			bool inPit = state.Current.PitThisLap || state.Current.StartedLapInPit || state.Current.GarageThisLap || (state.Current.LastLapEndPitState == PitState.EXITING && state.Current.LastPitLap == state.Current.LapsCompleted);
			if(!inPit)
				return;
			Pit pit = Pits.Count == 0 ? null : Pits[^1];
			if(!IsSamePit(pit, state)) {
				pit = new Pit(state.Current);
				Pits.Add(pit);
			} else
				pit.Merge(state.Current);
		}

		private bool IsSamePit(Pit pit, CarStateChange state) {
			int lap = state.Current.LapsCompleted + (state.Current.PitThisLap || state.Current.GarageThisLap ? 1 : 0);
			bool isGarage = pit != null && (pit.GarageInTime >= 0 || pit.GarageOutTime >= 0);
			if(pit == null || pit.Lap != lap || (!isGarage && pit.PitTime != state.Current.LastPitTime) || (isGarage && state.Current.LastPitTime > Math.Max(pit.GarageInTime, pit.GarageOutTime)))
				return false;
			if((!isGarage && state.Current.LastGarageInTime > pit.PitTime) || (isGarage && pit.GarageInTime != state.Current.LastGarageInTime))
				return false;
			return true;
		}

		private void AddStrategy(UpdateContext<CarHistory> context, List<Strategy> strategy) {
			if(strategy != null && strategy.Count > 0) {
				foreach(Strategy strat in strategy) {
					if(Pit.IsDefaultStrategy(strat))
						continue;
					Pit match = null;
					for(int i = Pits.Count - 1; i >= 0; i--) {
						Pit pit = Pits[i];
						if(strat.lap == 0 && pit.Lap == 1 && pit.GarageInTime >= 0) {
							if(context.Options.TraceLogging)
								context.Logger.LogDebug($"{Key.Id()} initial {strat.lap} {pit.Lap}");
							match = pit;
							break;
						} else if((strat.lap == pit.Lap && !pit.StopAfterLine) || (strat.lap == pit.Lap + 1 && pit.StopAfterLine)) {
							if(context.Options.TraceLogging)
								context.Logger.LogTrace($"{Key.Id()} matched pit {strat.lap} {pit.Lap}");
							match = pit;
							break;
						}
					}
					if(match == null) {
						match = new Pit() { Lap = strat.lap, Swap = strat.driverSwap };
						if(strat.lap != 0) {
							if(context.Options.TraceLogging)
								context.Logger.LogDebug($"{Key.Id()} unmatched strat {strat.lap}");
							match.StopTime = context.CurrentET - strat.time;
							match.ReleaseTime = context.CurrentET;
						} else {
							if(context.Options.TraceLogging)
								context.Logger.LogDebug($"{Key.Id()} unmatched initial {strat.lap}");
							match.GarageOutTime = context.CurrentET;
						}
						if(Pits.Count > 0 && match.Lap < Pits[^1].Lap) {
							if(context.Options.TraceLogging)
								context.Logger.LogInformation($"{Key.Id()} violation {strat.lap} < {Pits[^1].Lap}");
						} else
							Pits.Add(match);
					}
					if(match != null)
						match.Resolve(strat);
				}
			}
		}

		private void AddUsage(UpdateContext<CarHistory> context, List<StrategyDriverUsage> usage) {
			if(usage != null)
				foreach(StrategyDriverUsage lapUsage in usage) {
					if(lapUsage.lap > 0 && lapUsage.lap <= Laps.Count) {
						Lap lap = Laps[lapUsage.lap - 1];
						if(lap == null)
							continue;
						if(lap.LapNumber != lapUsage.lap) {
							if(context.Options.TraceLogging)
								context.Logger.LogWarning($"{Key.Id()} mismatch [{lap.LapNumber} to {lapUsage.lap}]");
							continue;
						}
						lap.Resolve(lapUsage);
					}
				}
		}

		public void FixLaps() {
			int maxlap = 0;
			foreach(Lap lap in Laps)
				if(lap.LapNumber > maxlap)
					maxlap = lap.LapNumber;
			Lap[] laps = new Lap[maxlap];
			foreach(Lap lap in Laps)
				if(laps[lap.LapNumber - 1] == null || laps[lap.LapNumber - 1].IsDefault())
					laps[lap.LapNumber - 1] = lap;
			Laps.Clear();
			for(int i = 0; i < laps.Length; i++)
				Laps.Add(laps[i] ?? Lap.Default(i + 1));
		}

		public void Combine(CarHistory from) {
			if(!Key.Equals(from.Key))
				throw new Exception("Key mismatch");
			Car.Merge(from.Car);
			FixLaps();
			from.FixLaps();
			MergeLaps(from.Laps);
		}

		private void MergeLaps(List<Lap> from) {
			Dictionary<int, Lap> laps = new Dictionary<int, Lap>();
			int maxlap = 0;
			foreach(Lap lap in Laps) {
				laps.Add(lap.LapNumber, lap);
				if(lap.LapNumber > maxlap)
					maxlap = lap.LapNumber;
			}
			foreach(Lap lap in from) {
				if(laps.TryGetValue(lap.LapNumber, out Lap existing)) {
					if(existing.HasNoTime()) {
						lap.Driver = string.IsNullOrEmpty(lap.Driver) ? existing.Driver : lap.Driver;
						laps[lap.LapNumber] = lap;
					} else {
						laps.Add(lap.LapNumber, lap);
					}
				}
				if(lap.LapNumber > maxlap)
					maxlap = lap.LapNumber;
			}
			Laps.Clear();
			for(int i = 1; i <= maxlap; i++) {
				if(laps.TryGetValue(i, out Lap lap))
					Laps.Add(lap);
				else
					Laps.Add(Lap.Default(i));
			}
		}

		public CarHistory Clone() {
			CarHistory car = new CarHistory(Key, new Car() {
				SlotId = Car.SlotId,
				Veh = Car.Veh,
				VehicleName = Car.VehicleName,
				TeamName = Car.TeamName,
				Class = Car.Class,
				Number = Car.Number,
				Id = Car.Id,
				Vehicle = Car.Vehicle
			});
			foreach(Lap lap in Laps) {
				car.Laps.Add(lap?.Clone());
			}
			foreach(Pit pit in Pits) {
				car.Pits.Add(pit.Clone());
			}
			return car;
		}
	}
}
