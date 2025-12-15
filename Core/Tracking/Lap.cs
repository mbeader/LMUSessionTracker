using LMUSessionTracker.Core.LMU;

namespace LMUSessionTracker.Core.Tracking {
	public class Lap {
		public int LapNumber { get; set; }
		public double TotalTime { get; set; }
		public double Sector1 { get; set; }
		public double Sector2 { get; set; }
		public double Sector3 { get; set; }
		public string Driver { get; set; }
		public long SteamId { get; set; }

		public Lap() { }

		public Lap(Standing standing) {
			LapNumber = standing.lapsCompleted;
			TotalTime = standing.lastLapTime;
			Sector1 = standing.lastSectorTime1;
			Sector2 = standing.lastSectorTime2 - standing.lastSectorTime1;
			Sector3 = standing.lastLapTime - standing.lastSectorTime2;
			Driver = standing.driverName;
		}

		public static Lap Default(int lap) {
			return new Lap() { LapNumber = lap, TotalTime = -1.0, Sector1 = -1.0, Sector2 = -1.0, Sector3 = -1.0 };
		}

		public bool IsDefault() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0 && Driver == null && SteamId == 0;
		}

		public bool HasNoTime() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0;
		}
	}
}
