using LMUSessionTracker.Core.LMU;
using System;

namespace LMUSessionTracker.Core.Tracking {
	public class Lap {
		public int LapNumber { get; set; }
		public double TotalTime { get; set; }
		public double Sector1 { get; set; }
		public double Sector2 { get; set; }
		public double Sector3 { get; set; }
		public string Driver { get; set; }
		public bool IsValid => TotalTime != -1.0 && Sector1 != -1.0 && Sector2 != -1.0 && Sector3 != -1.0;
		public int Position { get; set; }
		public bool Pit { get; set; }
		public double Fuel { get; set; }
		public double VirtualEnergy { get; set; }
		public double LFTire { get; set; }
		public double RFTire { get; set; }
		public double LRTire { get; set; }
		public double RRTire { get; set; }
		public string FinishStatus { get; set; }
		public DateTime? Timestamp { get; set; }

		public Lap() { }

		public Lap(Standing standing) {
			LapNumber = standing.lapsCompleted;
			TotalTime = standing.lastLapTime;
			Sector1 = standing.lastSectorTime1;
			Sector2 = standing.lastSectorTime2 > 0 && standing.lastSectorTime1 > 0 ? standing.lastSectorTime2 - standing.lastSectorTime1 : -1.0;
			Sector3 = standing.lastLapTime > 0 && standing.lastSectorTime2 > 0 ? standing.lastLapTime - standing.lastSectorTime2 : -1.0;
			Driver = standing.driverName;
			Position = standing.position;
			Fuel = standing.fuelFraction;
			FinishStatus = standing.finishStatus;
		}
		public static Lap Default(int lap) {
			return new Lap() { LapNumber = lap, TotalTime = -1.0, Sector1 = -1.0, Sector2 = -1.0, Sector3 = -1.0 };
		}

		public bool IsDefault() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0 && Driver == null;
		}

		public bool HasNoTime() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0;
		}
	}
}
