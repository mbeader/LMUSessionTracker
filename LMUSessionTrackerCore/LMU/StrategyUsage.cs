using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class StrategyUsage : Dictionary<string, List<StrategyDriverUsage>> {
	}

	public class StrategyDriverUsage {
		/// <summary>
		/// Player only
		/// </summary>
		public double fuel { get; set; }
		public int lap { get; set; }
		public bool pit { get; set; }
		public int stint { get; set; }
		/// <summary>
		/// Player only
		/// </summary>
		public List<double> tyres { get; set; }
		public double ve { get; set; }
	}
}
