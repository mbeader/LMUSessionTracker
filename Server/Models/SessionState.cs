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
	}
}
