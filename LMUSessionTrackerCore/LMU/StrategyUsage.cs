using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class StrategyUsage : Dictionary<string, List<StrategyDriverUsage>> {
	}

	public class StrategyDriverUsage {
		public int lap { get; set; }
		public bool pit { get; set; }
		public int stint { get; set; }
		public double ve { get; set; }
	}
}
