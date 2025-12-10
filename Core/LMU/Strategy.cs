namespace LMUSessionTracker.Core.LMU {
	public class Strategy {
		public string driver { get; set; }
		public bool driverSwap { get; set; }
		public int lap { get; set; }
		public bool penalty { get; set; }
		public double previousStintDuration { get; set; }
		public double time { get; set; }
		public StrategyTires tyres { get; set; }
		public double ve { get; set; }
	}
}
