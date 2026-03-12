namespace LMUSessionTracker.Core.Tracking {
	public class Best {
		public double Total { get; set; } = -1;
		public double Sector1 { get; set; } = -1;
		public double Sector2 { get; set; } = -1;
		public double Sector3 { get; set; } = -1;

		public bool Update(Lap lap) {
			bool changed = false;
			if(lap.TotalTime > 0 && (lap.TotalTime < Total || Total <= 0)) {
				Total = lap.TotalTime;
				changed = true;
			}
			if(lap.Sector1 > 0 && (lap.Sector1 < Sector1 || Sector1 <= 0)) {
				Sector1 = lap.Sector1;
				changed = true;
			}
			if(lap.Sector2 > 0 && (lap.Sector2 < Sector2 || Sector2 <= 0)) {
				Sector2 = lap.Sector2;
				changed = true;
			}
			if(lap.Sector3 > 0 && (lap.Sector3 < Sector3 || Sector3 <= 0)) {
				Sector3 = lap.Sector3;
				changed = true;
			}
			return changed;
		}
	}
}
