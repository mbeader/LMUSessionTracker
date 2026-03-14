using LMUSessionTracker.Common.LMU;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarHistory {
		private readonly List<Strategy> strats = new List<Strategy>();
		private int lastStrategy = -1;
		//private string lastSector = null;

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

		public Lap Update(UpdateContext<CarHistory> context, CarStateChange state, Standing standing, List<Strategy> strategy) {
			Lap newLap = null;
			if(standing.lapsCompleted > 0)
				newLap = AddLap(new Lap(standing, state.Previous), context.Timestamp);
			//if(lastSector != standing.sector) {
			//	if(lastSector != null)
			//		context.Logger.LogInformation($"{Key.Id()} {standing.sector}");
			//	lastSector = standing.sector;
			//}
			AddPit(state, newLap);
			AddStrategy(context, strategy);
			if(!Car.HasAllFields)
				Car.Merge(new Car(standing));
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
				if(strategy.Count < strats.Count) {
					context.Logger.LogInformation($"{Key.Id()} reset [{strats.Count} to {strategy.Count}]");
					strats.Clear();
				}
				for(int i = 0; i < strategy.Count; i++) {
					Strategy s = strategy[i];
					if(i >= strats.Count)
						strats.Add(s);
					else {
						List<string> diff = Diff(strats[i], s);
						if(diff.Count > 0)
							context.Logger.LogInformation($"{Key.Id()} ({i}) changed {string.Join(", ", diff)}");
						strats[i] = s;
					}
				}
			}
			if(strategy != null && strategy.Count > 0 && strategy[^1].lap > lastStrategy) {
				foreach(Strategy strat in strategy) {
					if(strat.lap > lastStrategy) {
						bool initialDefault = strat.lap == 0 && Pit.IsDefaultStrategy(strat);
						if(initialDefault)
							continue;
						Pit match = null;
						foreach(Pit pit in Pits) {
							if(strat.lap == 0 && pit.Lap == 1 && pit.GarageInTime >= 0) {
								context.Logger.LogInformation($"{Key.Id()} initial {strat.lap} {pit.Lap}");
								match = pit;
								break;
							} else if((strat.lap == pit.Lap && !pit.StopAfterLine) || (strat.lap == pit.Lap + 1 && pit.StopAfterLine)) {
								context.Logger.LogInformation($"{Key.Id()} matched pit {strat.lap} {pit.Lap}");
								match = pit;
								break;
							}
						}
						if(match == null) {
							match = new Pit() { Lap = strat.lap, Swap = strat.driverSwap };
							if(strat.lap != 0) {
								context.Logger.LogInformation($"{Key.Id()} unmatched strat {strat.lap}");
								match.StopTime = context.CurrentET - strat.time;
								match.ReleaseTime = context.CurrentET;
							} else {
								context.Logger.LogInformation($"{Key.Id()} unmatched initial {strat.lap}");
								match.GarageOutTime = context.CurrentET;
							}
							Pits.Add(match);
						}
						if(match != null)
							match.Resolve(strat);
						lastStrategy = strat.lap;
					}
				}
			}
		}

		private List<string> Diff(Strategy a, Strategy b) {
			List<string> res = new List<string>();
			if(a.driver != b.driver)
				res.Add($"driver: [{a.driver} to {b.driver}]");
			if(a.driverSwap != b.driverSwap)
				res.Add($"driverSwap: [{a.driverSwap} to {b.driverSwap}]");
			if(a.fuel != b.fuel)
				res.Add($"fuel: [{a.fuel} to {b.fuel}]");
			if(a.lap != b.lap)
				res.Add($"lap: [{a.lap} to {b.lap}]");
			if(a.penalty != b.penalty)
				res.Add($"penalty: [{a.penalty} to {b.penalty}]");
			if(a.previousStintDuration != b.previousStintDuration)
				res.Add($"previousStintDuration: [{a.previousStintDuration} to {b.previousStintDuration}]");
			if(a.time != b.time)
				res.Add($"time: [{a.time} to {b.time}]");
			//if(a.ve != b.ve)
			//	res.Add($"ve: [{a.ve} to {b.ve}]");
			if((a.tyres?.fl == null) != (b.tyres?.fl == null))
				res.Add($"tyres.fl: [{a.tyres?.fl == null} to {b.tyres?.fl == null}]");
			if((a.tyres?.fr == null) != (b.tyres?.fr == null))
				res.Add($"tyres.fr: [{a.tyres?.fr == null} to {b.tyres?.fr == null}]");
			if((a.tyres?.rl == null) != (b.tyres?.rl == null))
				res.Add($"tyres.rl: [{a.tyres?.rl == null} to {b.tyres?.rl == null}]");
			if((a.tyres?.rr == null) != (b.tyres?.rr == null))
				res.Add($"tyres.rr: [{a.tyres?.rr == null} to {b.tyres?.rr == null}]");
			if(a.tyres?.fl != null && b.tyres?.fl != null) {
				if(a.tyres.fl.changed != b.tyres.fl.changed)
					res.Add($"tyres.fl.changed: [{a.tyres.fl.changed} to {b.tyres.fl.changed}]");
				if(a.tyres.fl.compound != b.tyres.fl.compound)
					res.Add($"tyres.fl.compound: [{a.tyres.fl.compound} to {b.tyres.fl.compound}]");
				if(a.tyres.fl.New != b.tyres.fl.New)
					res.Add($"tyres.fl.New: [{a.tyres.fl.New} to {b.tyres.fl.New}]");
				if(a.tyres.fl.usage != b.tyres.fl.usage)
					res.Add($"tyres.fl.usage: [{a.tyres.fl.usage} to {b.tyres.fl.usage}]");
			}
			if(a.tyres?.fr != null && b.tyres?.fr != null) {
				if(a.tyres.fr.changed != b.tyres.fr.changed)
					res.Add($"tyres.fr.changed: [{a.tyres.fr.changed} to {b.tyres.fr.changed}]");
				if(a.tyres.fr.compound != b.tyres.fr.compound)
					res.Add($"tyres.fr.compound: [{a.tyres.fr.compound} to {b.tyres.fr.compound}]");
				if(a.tyres.fr.New != b.tyres.fr.New)
					res.Add($"tyres.fr.New: [{a.tyres.fr.New} to {b.tyres.fr.New}]");
				if(a.tyres.fr.usage != b.tyres.fr.usage)
					res.Add($"tyres.fr.usage: [{a.tyres.fr.usage} to {b.tyres.fr.usage}]");
			}
			if(a.tyres?.rl != null && b.tyres?.rl != null) {
				if(a.tyres.rl.changed != b.tyres.rl.changed)
					res.Add($"tyres.rl.changed: [{a.tyres.rl.changed} to {b.tyres.rl.changed}]");
				if(a.tyres.rl.compound != b.tyres.rl.compound)
					res.Add($"tyres.rl.compound: [{a.tyres.rl.compound} to {b.tyres.rl.compound}]");
				if(a.tyres.rl.New != b.tyres.rl.New)
					res.Add($"tyres.rl.New: [{a.tyres.rl.New} to {b.tyres.rl.New}]");
				if(a.tyres.rl.usage != b.tyres.rl.usage)
					res.Add($"tyres.rl.usage: [{a.tyres.rl.usage} to {b.tyres.rl.usage}]");
			}
			if(a.tyres?.rr != null && b.tyres?.rr != null) {
				if(a.tyres.rr.changed != b.tyres.rr.changed)
					res.Add($"tyres.rr.changed: [{a.tyres.rr.changed} to {b.tyres.rr.changed}]");
				if(a.tyres.rr.compound != b.tyres.rr.compound)
					res.Add($"tyres.rr.compound: [{a.tyres.rr.compound} to {b.tyres.rr.compound}]");
				if(a.tyres.rr.New != b.tyres.rr.New)
					res.Add($"tyres.rr.New: [{a.tyres.rr.New} to {b.tyres.rr.New}]");
				if(a.tyres.rr.usage != b.tyres.rr.usage)
					res.Add($"tyres.rr.usage: [{a.tyres.rr.usage} to {b.tyres.rr.usage}]");
			}
			return res;
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
