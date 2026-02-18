using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class Bests {
		public Dictionary<string, Best> Class { get; } = new Dictionary<string, Best>();
		public Dictionary<CarKey, Best> Car { get; } = new Dictionary<CarKey, Best>();
		public Dictionary<CarKey, Dictionary<string, Best>> Driver { get; } = new Dictionary<CarKey, Dictionary<string, Best>>();

		public Bests(List<CarHistory> history = null) {
			if(history != null) {
				foreach(CarHistory car in history) {
					foreach(Lap lap in car.Laps) {
						if(lap != null)
							Update(car.Car, lap);
					}
				}
			}
		}

		public bool Update(CarLap lap) {
			return Update(lap.Car, lap.Lap);
		}

		private bool Update(Car car, Lap lap) {
			bool classChanged = UpdateClass(car, lap);
			bool carChanged = UpdateCar(car, lap);
			return classChanged || carChanged;
		}

		private bool UpdateClass(Car car, Lap lap) {
			if(car.Class == null)
				return false;
			if(!Class.TryGetValue(car.Class, out Best classBest)) {
				classBest = new Best();
				Class.Add(car.Class, classBest);
			}
			return classBest.Update(lap);
		}

		private bool UpdateCar(Car car, Lap lap) {
			if(car.Veh == null)
				return false;
			CarKey key = new CarKey(car.SlotId, car.Veh);
			if(!Car.TryGetValue(key, out Best carBest)) {
				carBest = new Best();
				Car.Add(key, carBest);
			}
			bool carChanged = carBest.Update(lap);
			bool driverChanged = UpdateDriver(key, car, lap);
			return carChanged || driverChanged;
		}

		private bool UpdateDriver(CarKey key, Car car, Lap lap) {
			if(!Driver.TryGetValue(key, out Dictionary<string, Best> drivers)) {
				drivers = new Dictionary<string, Best>();
				Driver.Add(key, drivers);
			}

			if(!drivers.TryGetValue(lap.Driver, out Best driverBest)) {
				driverBest = new Best();
				drivers.Add(lap.Driver, driverBest);
			}
			return driverBest.Update(lap);
		}
	}
}
