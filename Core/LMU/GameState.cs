namespace LMUSessionTracker.Core.LMU {
	public class GameState {
		public string MultiStintState { get; set; }
		public double? PitEntryDist { get; set; }
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
