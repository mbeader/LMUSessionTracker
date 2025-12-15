using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMUSessionTracker.Server.Models {
	public class Session {
		[Key, Required]
		public string SessionId { get; set; }

		public DateTime Timestamp { get; set; }
		public bool IsClosed { get; set; }
		
		public double EndEventTime { get; set; }
		public string GameMode { get; set; }
		public double LapDistance { get; set; }
		public int MaxPlayers { get; set; }
		public double MaxTime { get; set; }
		public uint MaximumLaps { get; set; }
		public bool PasswordProtected { get; set; }
		public string ServerName { get; set; }
		public int ServerPort { get; set; }
		public string SessionType { get; set; }
		public double StartEventTime { get; set; }
		public string TrackName { get; set; }

		public SessionState LastState { get; }
		public ICollection<Lap> Laps { get; } = new List<Lap>();
		public ICollection<Entry> Entries { get; } = new List<Entry>();
		public ICollection<Member> Members { get; } = new List<Member>();
		public ICollection<Chat> Chats { get; } = new List<Chat>();

		public void From(SessionInfo info) {
			EndEventTime = info.endEventTime;
			GameMode = info.gameMode;
			LapDistance = info.lapDistance;
			MaxPlayers = info.maxPlayers;
			MaxTime = info.maxTime;
			MaximumLaps = info.maximumLaps;
			PasswordProtected = info.passwordProtected;
			ServerName = info.serverName;
			ServerPort = info.serverPort;
			SessionType = info.session;
			StartEventTime = info.startEventTime;
			TrackName = info.trackName;
		}
	}
}
