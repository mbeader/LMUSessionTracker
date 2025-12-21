using Newtonsoft.Json;

namespace LMUSessionTracker.Core.LMU {
	public class WeatherNode {
		[JsonProperty("Duration")]
		public int Duration { get; set; }
		[JsonProperty("Humidity")]
		public int Humidity { get; set; }
		[JsonProperty("RainChance")]
		public int RainChance { get; set; }
		[JsonProperty("Sky")]
		public int Sky { get; set; }
		[JsonProperty("StartTime")]
		public int StartTime { get; set; }
		[JsonProperty("Temperature")]
		public int Temperature { get; set; }
		[JsonProperty("WindDirection")]
		public int WindDirection { get; set; }
		[JsonProperty("WindSpeed")]
		public int WindSpeed { get; set; }
	}
}
