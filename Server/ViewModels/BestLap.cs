using LMUSessionTracker.Server.Models;

namespace LMUSessionTracker.Server.ViewModels {
	public class BestLap {
		public Lap Lap { get; set; }
		public Lap Sector1 { get; set; }
		public Lap Sector2 { get; set; }
		public Lap Sector3 { get; set; }
		public Pit LapPit { get; set; }
		public Pit Sector1Pit { get; set; }
		public Pit Sector2Pit { get; set; }
		public Pit Sector3Pit { get; set; }
		public Core.Tracking.Vehicle Vehicle { get; set; }
	}
}
