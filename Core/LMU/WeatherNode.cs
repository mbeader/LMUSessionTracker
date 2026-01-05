using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.LMU {
	public class WeatherNode {
		[JsonPropertyName("Duration")]
		public int Duration { get; set; }
		[JsonPropertyName("Humidity")]
		public int Humidity { get; set; }
		[JsonPropertyName("RainChance")]
		public int RainChance { get; set; }
		[JsonPropertyName("Sky")]
		public int Sky { get; set; }
		[JsonPropertyName("StartTime")]
		public int StartTime { get; set; }
		[JsonPropertyName("Temperature")]
		public int Temperature { get; set; }
		[JsonPropertyName("WindDirection")]
		public int WindDirection { get; set; }
		[JsonPropertyName("WindSpeed")]
		public int WindSpeed { get; set; }
	}
}
