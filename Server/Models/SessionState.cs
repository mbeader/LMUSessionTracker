using LMUSessionTracker.Core.LMU;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class SessionState {
		[Key, Required]
		public long SessionStateId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public Guid SessionId { get; set; }

		public DateTime Timestamp { get; set; }

		public double AmbientTemp { get; set; }
		public double AveragePathWetness { get; set; }
		public double CurrentEventTime { get; set; }
		public double DarkCloud { get; set; }
		public int? GamePhase { get; set; }
		public bool? InRealtime { get; set; }
		public double MaxPathWetness { get; set; }
		public double MinPathWetness { get; set; }
		public int? NumRedLights { get; set; }
		public int? NumberOfPlayers { get; set; }
		public int? NumberOfVehicles { get; set; }
		public double? RaceCompletion { get; set; }
		public double? Raining { get; set; }
		public string Sector1Flag { get; set; }
		public string Sector2Flag { get; set; }
		public string Sector3Flag { get; set; }
		public int? StartLightFrame { get; set; }
		public double? TimeRemainingInGamePhase { get; set; }
		public double? TrackTemp { get; set; }
		public double? WindVelocity { get; set; }
		public double? WindX { get; set; }
		public double? WindY { get; set; }
		public double? WindZ { get; set; }
		public string YellowFlagState { get; set; }

		public Session Session { get; set; }

		public void From(SessionInfo info) {
			AmbientTemp = info.ambientTemp;
			AveragePathWetness = info.averagePathWetness;
			CurrentEventTime = info.currentEventTime;
			DarkCloud = info.darkCloud;
			GamePhase = info.gamePhase;
			InRealtime = info.inRealtime;
			MaxPathWetness = info.maxPathWetness;
			MinPathWetness = info.minPathWetness;
			NumRedLights = info.numRedLights;
			NumberOfPlayers = info.numberOfPlayers;
			NumberOfVehicles = info.numberOfVehicles;
			RaceCompletion = info.raceCompletion?.timeCompletion;
			Raining = info.raining;
			Sector1Flag = info.sectorFlag?[0];
			Sector2Flag = info.sectorFlag?[1];
			Sector3Flag = info.sectorFlag?[2];
			StartLightFrame = info.startLightFrame;
			TimeRemainingInGamePhase = info.timeRemainingInGamePhase;
			TrackTemp = info.trackTemp;
			WindVelocity = info.windSpeed?.velocity;
			WindX = info.windSpeed?.x;
			WindY = info.windSpeed?.y;
			WindZ = info.windSpeed?.z;
			YellowFlagState = info.yellowFlagState;
		}
	}
}
