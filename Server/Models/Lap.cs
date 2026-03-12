using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Lap {
		[Key, Required]
		public long LapId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }
		[ForeignKey(nameof(Car)), Required]
		public long CarId { get; set; }

		[Required]
		public int LapNumber { get; set; }
		public string Driver { get; set; }
		public string FinishStatus { get; set; }
		public double StartTime { get; set; }
		public double TotalTime { get; set; }
		public double Sector1 { get; set; }
		public double Sector2 { get; set; }
		public double Sector3 { get; set; }
		public bool IsValid { get; set; }
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

		public DateTime? Timestamp { get; set; }

		public Session Session { get; set; }
		public Car Car { get; set; }

		[NotMapped]
		public bool Known { get; set; }

		public void From(Core.Tracking.Lap lap) {
			if(LapNumber > 0 && LapNumber != lap.LapNumber)
				throw new InvalidOperationException($"Cannot change lap from L{LapNumber} to L{lap.LapNumber}");
			LapNumber = lap.LapNumber;
			Driver = lap.Driver;
			FinishStatus = lap.FinishStatus;
			StartTime = lap.StartTime;
			TotalTime = lap.TotalTime;
			Sector1 = lap.Sector1;
			Sector2 = lap.Sector2;
			Sector3 = lap.Sector3;
			IsValid = lap.IsValid;
			Position = lap.Position;
			Penalty = lap.Penalty;
			Garage = lap.Garage;
			Pit = lap.Pit;
			Fuel = lap.Fuel;
			VirtualEnergy = lap.VirtualEnergy;
			LFTire = lap.LFTire;
			RFTire = lap.RFTire;
			LRTire = lap.LRTire;
			RRTire = lap.RRTire;
			Timestamp = lap.Timestamp;
		}

		public Core.Tracking.Lap To() {
			return new Core.Tracking.Lap() {
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
	}
}
