using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Pit {
		[Key, Required]
		public long PitId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }
		[ForeignKey(nameof(Car)), Required]
		public long CarId { get; set; }

		public int Lap { get; set; }
		public double PitTime { get; set; }
		public double StopTime { get; set; }
		public double ReleaseTime { get; set; }
		public double ExitTime { get; set; }
		public double GarageInTime { get; set; }
		public double GarageOutTime { get; set; }
		public double SwapTime { get; set; }
		public bool StopAfterLine { get; set; }
		public int StopLocation { get; set; }
		public bool Swap { get; set; }
		public int SwapLocation { get; set; }
		public bool Penalty { get; set; }
		public double Fuel { get; set; }
		public double VirtualEnergy { get; set; }
		public bool LFChanged { get; set; }
		public string LFCompound { get; set; }
		public bool LFNew { get; set; }
		public double LFUsage { get; set; }
		public bool RFChanged { get; set; }
		public string RFCompound { get; set; }
		public bool RFNew { get; set; }
		public double RFUsage { get; set; }
		public bool LRChanged { get; set; }
		public string LRCompound { get; set; }
		public bool LRNew { get; set; }
		public double LRUsage { get; set; }
		public bool RRChanged { get; set; }
		public string RRCompound { get; set; }
		public bool RRNew { get; set; }
		public double RRUsage { get; set; }
		public double PreviousStintDuration { get; set; }
		public double Time { get; set; }

		public Session Session { get; set; }
		public Car Car { get; set; }

		public void From(Core.Tracking.Pit pit) {
			if(Lap > 0 && Lap != pit.Lap)
				throw new InvalidOperationException($"Cannot change lap from L{Lap} to L{pit.Lap}");
			Lap = pit.Lap;
			PitTime = pit.PitTime;
			StopTime = pit.StopTime;
			ReleaseTime = pit.ReleaseTime;
			ExitTime = pit.ExitTime;
			GarageInTime = pit.GarageInTime;
			GarageOutTime = pit.GarageOutTime;
			SwapTime = pit.SwapTime;
			StopAfterLine = pit.StopAfterLine;
			Swap = pit.Swap;
			SwapLocation = pit.SwapLocation;
			Penalty = pit.Penalty;
			Fuel = pit.Fuel;
			VirtualEnergy = pit.VirtualEnergy;
			LFChanged = pit.LFChanged;
			LFCompound = pit.LFCompound;
			LFNew = pit.LFNew;
			LFUsage = pit.LFUsage;
			RFChanged = pit.RFChanged;
			RFCompound = pit.RFCompound;
			RFNew = pit.RFNew;
			RFUsage = pit.RFUsage;
			LRChanged = pit.LRChanged;
			LRCompound = pit.LRCompound;
			LRNew = pit.LRNew;
			LRUsage = pit.LRUsage;
			RRChanged = pit.RRChanged;
			RRCompound = pit.RRCompound;
			RRNew = pit.RRNew;
			RRUsage = pit.RRUsage;
			PreviousStintDuration = pit.PreviousStintDuration;
			Time = pit.Time;
		}

		public Core.Tracking.Pit To() {
			return new Core.Tracking.Pit() {
				Lap = Lap,
				PitTime = PitTime,
				StopTime = StopTime,
				ReleaseTime = ReleaseTime,
				ExitTime = ExitTime,
				GarageInTime = GarageInTime,
				GarageOutTime = GarageOutTime,
				SwapTime = SwapTime,
				StopAfterLine = StopAfterLine,
				Swap = Swap,
				SwapLocation = SwapLocation,
				Penalty = Penalty,
				Fuel = Fuel,
				VirtualEnergy = VirtualEnergy,
				LFChanged = LFChanged,
				LFCompound = LFCompound,
				LFNew = LFNew,
				LFUsage = LFUsage,
				RFChanged = RFChanged,
				RFCompound = RFCompound,
				RFNew = RFNew,
				RFUsage = RFUsage,
				LRChanged = LRChanged,
				LRCompound = LRCompound,
				LRNew = LRNew,
				LRUsage = LRUsage,
				RRChanged = RRChanged,
				RRCompound = RRCompound,
				RRNew = RRNew,
				RRUsage = RRUsage,
				PreviousStintDuration = PreviousStintDuration,
				Time = Time,
			};
		}
	}
}
