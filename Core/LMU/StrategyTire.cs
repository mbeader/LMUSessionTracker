using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.LMU {
	public class StrategyTire {
		public bool changed { get; set; }
		public string compound { get; set; }
		[JsonPropertyName("new")]
		public bool New { get; set; }
		public double? usage { get; set; }
	}
}
