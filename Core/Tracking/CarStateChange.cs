namespace LMUSessionTracker.Core.Tracking {
	public class CarStateChange {
		public CarState Previous { get; private set; }
		public CarState Current { get; private set; }

		public CarStateChange(CarState state = null) {
			Current = state;
		}

		public void Next(CarState state) {
			Previous = Current;
			Current = state;
		}
	}
}
