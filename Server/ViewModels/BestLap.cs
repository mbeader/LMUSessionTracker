using LMUSessionTracker.Server.Models;

namespace LMUSessionTracker.Server.ViewModels {
	public class BestLap {
		public Lap Lap { get; set; }
		public Lap Sector1 { get; set; }
		public Lap Sector2 { get; set; }
		public Lap Sector3 { get; set; }
		public Core.Tracking.Vehicle Vehicle { get; set; }
	}
}
