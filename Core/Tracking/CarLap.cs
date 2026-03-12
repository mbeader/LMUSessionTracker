namespace LMUSessionTracker.Core.Tracking {
	public class CarLap {
		public Car Car { get; init; }
		public Lap Lap { get; init; }

		public CarLap() { }

		public CarLap(Car car, Lap lap) {
			Car = car;
			Lap = lap;
		}
	}
}
