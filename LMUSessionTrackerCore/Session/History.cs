using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session {
	public class History {
		private readonly Dictionary<CarKey, CarHistory> cars = new Dictionary<CarKey, CarHistory>();

		public History(List<CarHistory> history = null) {
			if(history != null) 				foreach(CarHistory car in history)
					cars.Add(new CarKey() { SlotId = car.Car.SlotId, Veh = car.Car.Veh ?? "" }, car);
		}

		public void Update(List<Standing> standings) {
			foreach(Standing standing in standings)
				Update(standing);
		}

		private void Update(Standing standing) {
			CarKey key = new CarKey() { SlotId = standing.slotID, Veh = standing.vehicleFilename };
			if(!cars.TryGetValue(key, out CarHistory car)) {
				car = new CarHistory(key, new Car(standing));
				cars.Add(key, car);
			}
			car.Update(standing);
		}

		public List<CarHistory> GetAllHistory() {
			List<CarHistory> res = new List<CarHistory>(cars.Values);
			res.Sort((a, b) =>
				a.Car.Class != b.Car.Class ? ClassId(a.Car.Class).CompareTo(ClassId(b.Car.Class)) :
				a.Car.Number != b.Car.Number ? string.Compare(a.Car.Number, b.Car.Number, StringComparison.OrdinalIgnoreCase) :
				a.Car.TeamName != b.Car.TeamName ? string.Compare(a.Car.TeamName, b.Car.TeamName, StringComparison.OrdinalIgnoreCase) :
				string.Compare(a.Car.VehicleName, b.Car.VehicleName, StringComparison.OrdinalIgnoreCase)
			);
			return res;
		}

		private int ClassId(string s) {
			if(s.Contains("Hyper", StringComparison.OrdinalIgnoreCase))
				return 1;
			else if(s.Contains("LMP2", StringComparison.OrdinalIgnoreCase))
				return 2;
			else if(s.Contains("LMP3", StringComparison.OrdinalIgnoreCase))
				return 3;
			else if(s.Contains("GTE", StringComparison.OrdinalIgnoreCase))
				return 4;
			else if(s.Contains("GT3", StringComparison.OrdinalIgnoreCase))
				return 5;
			else
				return 6;
		}

		public void Clear() {
			cars.Clear();
		}
	}
}
