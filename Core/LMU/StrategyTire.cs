using Newtonsoft.Json;

namespace LMUSessionTracker.Core.LMU {
	public class StrategyTire {
		public bool changed { get; set; }
		public string compound { get; set; }
		[JsonProperty("new")]
		public bool New { get; set; }
	}
}
