using LMUSessionTracker.CoreServer.Tracking;

namespace LMUSessionTracker.Server.ViewModels {
	public class LapsViewModel {
		private double? currentET = null;

		public CarHistory Car { get; set; }
		public Entry Entry { get; set; }
		public SessionSummary Session { get; set; }
		public Bests Bests { get; set; }
		public double CurrentET {
			get {
				return currentET ?? Session?.CurrentET ?? -1;
			}
			set {
				currentET = value;
			}
		}
	}
}
