using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarHistory {
		public CarKey Key { get; private set; }
		public Car Car { get; private set; }
		public List<Lap> Laps { get; } = new List<Lap>();
		public int LapsCompleted { get; private set; }

		public CarHistory(CarKey key, Car car, List<Lap> laps = null) {
			Key = key;
			Car = car;
			if(laps != null) {
				Laps.AddRange(laps);
				LapsCompleted = Laps.Count;
			}
		}

		private void AddLap(Lap lap, DateTime timestamp) {
			while(lap.LapNumber > Laps.Count)
				Laps.Add(null);
			if(Laps[lap.LapNumber - 1] == null) {
				Laps[lap.LapNumber - 1] = lap;
				lap.Timestamp = timestamp;
			}
			LapsCompleted = Laps.Count;
		}

		public Lap GetLap(int lapNumber) {
			if(lapNumber > Laps.Count)
				return null;
			return Laps[lapNumber - 1] ?? Lap.Default(lapNumber);
		}

		public void Update(Standing standing, DateTime timestamp) {
			//if(standing.lapsCompleted > 0 && (Laps.Count == 0 || standing.lapsCompleted > Laps[^1].LapNumber)) {
			//	while(Laps.Count <= standing.lapsCompleted)
			//		Laps.Add(Lap.Default(Laps.Count + 1));
			//	Laps.Add(new Lap(standing));
			//}
			if(standing.lapsCompleted > 0)
				AddLap(new Lap(standing), timestamp);
			else
				Car.Merge(new Car(standing));
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
			});
			foreach(Lap lap in Laps) {
				car.Laps.Add(new Lap() {
					LapNumber = lap.LapNumber,
					TotalTime = lap.TotalTime,
					Sector1 = lap.Sector1,
					Sector2 = lap.Sector2,
					Sector3 = lap.Sector3,
					Driver = lap.Driver,
				});
			}
			return car;
		}
	}
}
