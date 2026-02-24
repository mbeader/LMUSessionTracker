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
		public bool IsValid => TotalTime != -1.0;
		public int Position { get; set; }
		public bool Penalty { get; set; }
		public bool Garage { get; set; }
		public bool Pit { get; set; }
		public double Fuel { get; set; }
		public double VirtualEnergy { get; set; }
		public double LFTire { get; set; }
		public double RFTire { get; set; }
		public double LRTire { get; set; }
		public double RRTire { get; set; }
		public string FinishStatus { get; set; }
		public double StartTime { get; set; }
		public DateTime? Timestamp { get; set; }

		public Lap() { }

		public Lap(Standing standing, CarState state = null) {
			LapNumber = standing.lapsCompleted;
			TotalTime = standing.lastLapTime;
			Sector1 = standing.lastSectorTime1;
			Sector2 = standing.lastSectorTime2 > 0 && standing.lastSectorTime1 > 0 ? standing.lastSectorTime2 - standing.lastSectorTime1 : -1.0;
			Sector3 = standing.lastLapTime > 0 && standing.lastSectorTime2 > 0 ? standing.lastLapTime - standing.lastSectorTime2 : -1.0;
			Driver = standing.driverName;
			Position = standing.position;
			Fuel = standing.fuelFraction;
			FinishStatus = standing.finishStatus;
			StartTime = state?.LapStartET ?? -1;
			Penalty = state?.PenaltyThisLap ?? false;
			Garage = state?.GarageThisLap ?? false;
			Pit = state?.PitThisLap ?? false;
		}

		public Lap Clone() {
			return new Lap() {
				LapNumber = LapNumber,
				TotalTime = TotalTime,
				Sector1 = Sector1,
				Sector2 = Sector2,
				Sector3 = Sector3,
				Driver = Driver,
				Position = Position,
				Penalty = Penalty,
				Garage = Garage,
				Pit = Pit,
				Fuel = Fuel,
				VirtualEnergy = VirtualEnergy,
				LFTire = LFTire,
				RFTire = RFTire,
				LRTire = LRTire,
				RRTire = RRTire,
				FinishStatus = FinishStatus,
				StartTime = StartTime,
				Timestamp = Timestamp,
			};
		}

		public static Lap Default(int lap) {
			return new Lap() { LapNumber = lap, TotalTime = -1.0, Sector1 = -1.0, Sector2 = -1.0, Sector3 = -1.0, StartTime = -1.0 };
		}

		public bool IsDefault() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0 && Driver == null;
		}

		public bool HasNoTime() {
			return TotalTime == -1.0 && Sector1 == -1.0 && Sector2 == -1.0 && Sector3 == -1.0;
		}
	}
}
