using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Server.Models {
	public class Session {
		[Key, Required]
		public string SessionId { get; set; }

		public DateTime Timestamp { get; set; }
		public bool IsOnline { get; set; }
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

		[JsonIgnore]
		public SessionState LastState { get; set; }
		[JsonIgnore]
		public ICollection<SessionTransition> PastSessions { get; } = new List<SessionTransition>();
		[JsonIgnore]
		public ICollection<SessionTransition> FutureSessions { get; } = new List<SessionTransition>();
		[JsonIgnore]
		public ICollection<Car> Cars { get; } = new List<Car>();
		[JsonIgnore]
		public ICollection<CarState> CarStates { get; } = new List<CarState>();
		[JsonIgnore]
		public ICollection<Lap> Laps { get; } = new List<Lap>();
		[JsonIgnore]
		public ICollection<Entry> Entries { get; } = new List<Entry>();
		[JsonIgnore]
		public ICollection<Member> Members { get; } = new List<Member>();
		[JsonIgnore]
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

		public Core.Tracking.Session To() {
			SessionInfo info = AsSessionInfo();
			Core.Tracking.EntryList entries = null;
			if(Entries != null && Entries.Count > 0) {
				List<Core.Tracking.Entry> coreEntries = new List<Core.Tracking.Entry>();
				foreach(Entry entry in Entries)
					coreEntries.Add(entry.To());
				entries = new Core.Tracking.EntryList(coreEntries);
			} else if(IsOnline)
				entries = new Core.Tracking.EntryList();
			List<Core.Tracking.CarHistory> carHistories = null;
			List<Core.Tracking.CarState> carStates = null;
			if(Cars != null) {
				carHistories = new List<Core.Tracking.CarHistory>();
				carStates = new List<Core.Tracking.CarState>();
				foreach(Car car in Cars) {
					Core.Tracking.CarKey key = new Core.Tracking.CarKey() { SlotId = car.SlotId, Veh = car.Veh };
					List<Core.Tracking.Lap> laps = new List<Core.Tracking.Lap>();
					foreach(Lap lap in car.Laps)
						laps.Add(lap.To());
					carHistories.Add(new Core.Tracking.CarHistory(key, car.To(), laps));
					if(car.LastState != null)
						carStates.Add(car.LastState.To(key));
				}
			}
			Core.Tracking.Session session = Core.Tracking.Session.Create(SessionId, info, Timestamp, entries, carHistories, carStates);
			if(IsClosed)
				session.Close();
			return session;
		}

		private SessionInfo AsSessionInfo() {
			return new SessionInfo() {
				ambientTemp = LastState.AmbientTemp,
				averagePathWetness = LastState.AveragePathWetness,
				currentEventTime = LastState.CurrentEventTime,
				darkCloud = LastState.DarkCloud,
				endEventTime = EndEventTime,
				gameMode = GameMode,
				gamePhase = LastState.GamePhase ?? -1,
				inRealtime = LastState.InRealtime ?? false,
				lapDistance = LapDistance,
				maxPathWetness = LastState.MaxPathWetness,
				maxPlayers = MaxPlayers,
				maxTime = MaxTime,
				maximumLaps = MaximumLaps,
				minPathWetness = LastState.MinPathWetness,
				numRedLights = LastState.NumRedLights ?? -1,
				numberOfPlayers = LastState.NumberOfPlayers ?? -1,
				numberOfVehicles = LastState.NumberOfVehicles ?? -1,
				passwordProtected = PasswordProtected,
				raceCompletion = new Completion() { timeCompletion = LastState.RaceCompletion ?? -1 },
				raining = LastState.Raining ?? -1,
				sectorFlag = new List<string>() { LastState.Sector1Flag, LastState.Sector2Flag, LastState.Sector3Flag },
				serverName = ServerName,
				serverPort = ServerPort,
				session = SessionType,
				startEventTime = StartEventTime,
				startLightFrame = LastState.StartLightFrame ?? -1,
				timeRemainingInGamePhase = LastState.TimeRemainingInGamePhase ?? -1,
				trackName = TrackName,
				trackTemp = LastState.TrackTemp ?? -1,
				windSpeed = new Velocity() {
					velocity = LastState.WindVelocity ?? -1,
					x = LastState.WindX ?? -1,
					y = LastState.WindY ?? -1,
					z = LastState.WindZ ?? -1,
				},
				yellowFlagState = LastState.YellowFlagState,
			};
		}
	}
}
