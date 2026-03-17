using LMUSessionTracker.Common;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMUSessionTracker.Core.Tracking {
	public class History {
		private readonly Dictionary<CarKey, CarHistory> cars = new Dictionary<CarKey, CarHistory>();

		public int Count => cars.Count;

		public History(List<CarHistory> history = null, EntryList entries = null) {
			if(history != null)
				foreach(CarHistory car in history)
					cars[new CarKey() { SlotId = car.Car.SlotId, Veh = car.Car.Veh ?? "" }] = car;
			if(entries != null)
				LoadEntryList(entries);
		}

		private void LoadEntryList(EntryList entries) {
			foreach(int slotId in entries.Slots.Keys) {
				Entry entry = entries.Slots[slotId];
				CarKey key = new CarKey() { SlotId = slotId, Veh = entry.Vehicle };
				Car car = new Car() {
					SlotId = slotId,
					Veh = entry.Vehicle,
					TeamName = entry.Name,
					Number = entry.Number,
					Id = entry.Id,
				};
				if(cars.TryGetValue(key, out CarHistory carHistory)) {
					carHistory.Car.TeamName = car.TeamName;
					carHistory.Car.Number = car.Number;
					carHistory.Car.Id = car.Id;
				} else {
					carHistory = new CarHistory(key, car);
					cars.Add(key, carHistory);
				}
			}
		}

		/// <summary>
		/// Returns the laps completed during this update
		/// </summary>
		public List<CarLap> Update(UpdateContext<History> context, CarStateMonitor carStates, List<Standing> standings, List<TeamStrategy> strategies, StrategyUsage usage) {
			Dictionary<string, List<List<Strategy>>> teamStrategies = new Dictionary<string, List<List<Strategy>>>();
			if(strategies != null)
				foreach(TeamStrategy strategy in strategies) {
					if(!teamStrategies.TryGetValue(strategy.Name, out List<List<Strategy>> teamStrategy)) {
						teamStrategy = new List<List<Strategy>>();
						teamStrategies.Add(strategy.Name, teamStrategy);
					}
					teamStrategy.Add(strategy.Strategy);
				}
			UpdateContext<CarHistory> carContext = context.Create<CarHistory>();
			List<CarLap> laps = new List<CarLap>();
			foreach(Standing standing in standings) {
				CarLap lap = Update(carContext, carStates, standing, teamStrategies, usage);
				if(lap != null)
					laps.Add(lap);
			}
			return laps;
		}

		private CarLap Update(UpdateContext<CarHistory> context, CarStateMonitor carStates, Standing standing, Dictionary<string, List<List<Strategy>>> strategies, StrategyUsage usage) {
			CarKey key = new CarKey() { SlotId = standing.slotID, Veh = standing.vehicleFilename };
			if(!cars.TryGetValue(key, out CarHistory car)) {
				car = new CarHistory(key, new Car(standing));
				cars.Add(key, car);
			}
			List<List<Strategy>> possibleStrategies = strategies.GetValueOrDefault(car.Car.TeamName);
			List<Strategy> strategy = null;
			if(possibleStrategies != null)
				foreach(List<Strategy> possibleStrategy in possibleStrategies) {
					if(possibleStrategy.Exists(x => x.driver == standing.driverName)) {
						strategy = possibleStrategy;
						break;
					}
				}
			Lap lap = car.Update(context, carStates.GetState(key), standing, strategy, usage?.GetValueOrDefault(standing.driverName));
			if(lap != null)
				return new CarLap() { Car = car.Car, Lap = lap };
			return null;
		}

		public void UpdateCars(EntryList entries) {
			foreach(int slotId in entries.Slots.Keys) {
				Entry entry = entries.Slots[slotId];
				CarKey key = new CarKey() { SlotId = slotId, Veh = entry.Vehicle };
				if(cars.TryGetValue(key, out CarHistory carHistory) && !carHistory.Car.HasAllFields) {
					carHistory.Car.Merge(new Car() {
						VehicleName = carHistory.Car.VehicleName,
						TeamName = entry.Name,
						Class = carHistory.Car.Class,
						Number = entry.Number,
						Id = entry.Id
					});
				}
			}
		}

		public void ResolveVehicles(VehicleService vehService) {
			foreach(CarHistory car in cars.Values) {
				if(car.Car.Vehicle == null)
					car.Car.Vehicle = vehService.GetVehicle(car.Car.Veh);
			}
		}

		public List<CarHistory> GetAllHistory() {
			List<CarHistory> res = new List<CarHistory>(cars.Values);
			NaturalSortStringComparer comparer = NaturalSortStringComparer.OrdinalIgnoreCase;
			res.Sort((a, b) =>
				a.Car.Class != b.Car.Class ? ClassId(a.Car.Class).CompareTo(ClassId(b.Car.Class)) :
				a.Car.Number != b.Car.Number ? comparer.Compare(a.Car.Number, b.Car.Number) :
				a.Car.TeamName != b.Car.TeamName ? comparer.Compare(a.Car.TeamName, b.Car.TeamName) :
				comparer.Compare(a.Car.VehicleName, b.Car.VehicleName)
			);
			return res;
		}

		private int ClassId(string s) {
			if(s == null)
				return 8;
			else if(s.Contains("Hyper", StringComparison.OrdinalIgnoreCase))
				return 1;
			else if(s.Contains("LMP2_ELMS", StringComparison.OrdinalIgnoreCase))
				return 2;
			else if(s.Contains("LMP2", StringComparison.OrdinalIgnoreCase))
				return 3;
			else if(s.Contains("LMP3", StringComparison.OrdinalIgnoreCase))
				return 4;
			else if(s.Contains("GTE", StringComparison.OrdinalIgnoreCase))
				return 5;
			else if(s.Contains("GT3", StringComparison.OrdinalIgnoreCase))
				return 6;
			else
				return 7;
		}

		public void Clear() {
			cars.Clear();
		}
	}
}
