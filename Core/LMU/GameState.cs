using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.LMU {
	public class GameState {
		[JsonPropertyName("MultiStintState")]
		public string MultiStintState { get; set; }
		[JsonPropertyName("PitEntryDist")]
		public double? PitEntryDist { get; set; }
		[JsonPropertyName("PitState")]
		public string PitState { get; set; }
		public WeatherNode closeestWeatherNode { get; set; }
		public string gamePhase { get; set; }
		public bool haveILoadedAllMyTiresYet { get; set; }
		public bool inControlOfVehicle { get; set; }
		public bool inMonitor { get; set; }
		public bool isReplayActive { get; set; }
		public bool playerVehicleLoaded { get; set; }
		public bool raceFinished { get; set; }
		public string teamVehicleState { get; set; }
		public double timeOfDay { get; set; }
	}
}
