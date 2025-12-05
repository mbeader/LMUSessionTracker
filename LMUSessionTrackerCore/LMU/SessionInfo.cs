using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class SessionInfo {
		public double ambientTemp { get; set; }
		public double averagePathWetness { get; set; }
		public double currentEventTime { get; set; }
		public double darkCloud { get; set; }
		public double endEventTime { get; set; }
		public string gameMode { get; set; }
		public int gamePhase { get; set; }
		public bool inRealtime { get; set; }
		public double lapDistance { get; set; }
		public double maxPathWetness { get; set; }
		public int maxPlayers { get; set; }
		public double maxTime { get; set; }
		public uint maximumLaps { get; set; }
		public double minPathWetness { get; set; }
		public int numRedLights { get; set; }
		public int numberOfPlayers { get; set; }
		public int numberOfVehicles { get; set; }
		public bool passwordProtected { get; set; }
		public string playerFileName { get; set; }
		public string playerName { get; set; }
		public Completion raceCompletion { get; set; }
		public double raining { get; set; }
		public List<string> sectorFlag { get; set; }
		public string serverName { get; set; }
		public int serverPort { get; set; }
		public string session { get; set; }
		public double startEventTime { get; set; }
		public int startLightFrame { get; set; }
		public double timeRemainingInGamePhase { get; set; }
		public string trackName { get; set; }
		public double trackTemp { get; set; }
		public Velocity windSpeed { get; set; }
		public string yellowFlagState { get; set; }

		public string PhaseName() {
			switch(gamePhase) {
				case 0: return "Starting";
				case 1: return "Reconnaissance laps";
				case 2: return "Grid";
				case 3: return "Formation lap";
				case 4: return "Countdown";
				case 5: return "Green";
				case 6: return "FCY";
				case 7: return "Session stopped";
				case 8: return "Checkered";
				case 9: return "Paused";
				default: return $"Unknown ({gamePhase})";
			}
		}
	}

}
