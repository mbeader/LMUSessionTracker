using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class TeamStrategy {
		// [name, strategy]
		public string Name { get; set; }
		public List<Strategy> Strategy { get; set; }
	}
}
