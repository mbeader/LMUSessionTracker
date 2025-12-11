using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Lap {
		[Key, Required]
		public long LapId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public Guid SessionId { get; set; }

		[Required]
		public int SlotId { get; set; }
		[Required]
		public string Veh { get; set; }
		[Required]
		public int LapNumber { get; set; }
		public string CarClass { get; set; }
		public string Vehicle { get; set; }
		public string Team { get; set; }
		public string Driver { get; set; }
		public string FinishStatus { get; set; }
		public double TotalTime { get; set; }
		public double Sector1 { get; set; }
		public double Sector2 { get; set; }
		public double Sector3 { get; set; }
		public bool IsValid { get; set; }
		public int Position { get; set; }
		public bool Pit { get; set; }
		public double Fuel { get; set; }
		public double VirtualEnergy { get; set; }
		public double LFTire { get; set; }
		public double RFTire { get; set; }
		public double LRTire { get; set; }
		public double RRTire { get; set; }

		public DateTime? Timestamp { get; set; }

		public Session Session { get; set; }
	}
}
