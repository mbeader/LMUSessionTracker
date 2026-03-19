using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class BestLapsViewModel {
		public List<BestLap> Laps { get; set; }
		public Dictionary<string, ClassBest> ClassBests { get; set; }
	}
}
