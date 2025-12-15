using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarHistory {
		public CarKey Key { get; private set; }
		public Car Car { get; private set; }
		public List<Lap> Laps { get; } = new List<Lap>();

		public CarHistory(CarKey key, Car car) {
			Key = key;
			Car = car;
		}

		public void Update(Standing standing) {
			if(standing.lapsCompleted > 0 && (Laps.Count == 0 || standing.lapsCompleted > Laps[^1].LapNumber)) {
				while(Laps.Count <= standing.lapsCompleted)
					Laps.Add(Lap.Default(Laps.Count + 1));
				Laps.Add(new Lap(standing));
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
						lap.SteamId = lap.SteamId <= 0 ? existing.SteamId : lap.SteamId;
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
	}
}
