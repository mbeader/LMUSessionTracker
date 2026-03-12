using LMUSessionTracker.Core.Tracking;

namespace LMUSessionTracker.Server.ViewModels {
	public class Result {
		public Car Car { get; set; }
		public CarState CarState { get; set; }
		public Lap BestLap { get; set; }
		public Lap LastLap { get; set; }
	}
}
