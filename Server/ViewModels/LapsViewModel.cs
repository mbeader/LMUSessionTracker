using LMUSessionTracker.Core.Tracking;

namespace LMUSessionTracker.Server.ViewModels {
	public class LapsViewModel {
		private double? currentET = null;

		public CarHistory Car { get; set; }
		public Session Session { get; set; }
		public Bests Bests { get; set; }
		public double CurrentET {
			get {
				return currentET ?? Session?.LastInfo?.currentEventTime ?? -1;
			}
			set {
				currentET = value;
			}
		}
	}
}
