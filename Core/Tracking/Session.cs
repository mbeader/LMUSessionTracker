using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class Session {
		public string SessionId { get; private set; }
		public string PrimaryClientId { get; private set; }
		public List<string> SecondaryClientIds { get; private set; }
		public string Track { get; private set; }
		public string Type { get; private set; }
		public bool Online { get; private set; }
		public Dictionary<string, bool> RoleChanges { get; private set; }
		public SessionInfo LastInfo { get; private set; }
		public List<Standing> LastStandings { get; private set; }
		public EntryList Entries { get; private set; }
		public History History { get; private set; }
		public DateTime LastUpdate { get; private set; }
		public bool Finished { get; private set; }

		private Session(string sessionId, SessionInfo info, EntryList entries, List<CarHistory> history) {
			SessionId = sessionId;
			SecondaryClientIds = new List<string>();
			Track = info.trackName;
			Type = info.session;
			RoleChanges = new Dictionary<string, bool>();
			LastInfo = info;
			Entries = entries;
			History = new History(history, entries);
			Finished = IsFinished(info);
		}

		public static Session Create(string sessionId, SessionInfo info, MultiplayerTeams teams = null, List<CarHistory> history = null) {
			EntryList entries = new EntryList(teams);
			return new Session(sessionId, info, entries, history) {
				Online = teams != null
			};
		}

		public static Session Create(string sessionId, SessionInfo info, EntryList entries, List<CarHistory> history) {
			return new Session(sessionId, info, entries, history) {
				Online = entries != null
			};
		}

		public bool RegisterClient(string clientId) {
			if(string.IsNullOrEmpty(clientId))
				throw new ArgumentException("Cannot be null or empty", nameof(clientId));
			if(PrimaryClientId != clientId || !SecondaryClientIds.Contains(clientId)) {
				if(PrimaryClientId == null) {
					PrimaryClientId = clientId;
				} else {
					SecondaryClientIds.Add(clientId);
				}
			}
			return PrimaryClientId == clientId;
		}

		public void UnregisterClient(string clientId) {
			if(string.IsNullOrEmpty(clientId))
				throw new ArgumentException("Cannot be null or empty", nameof(clientId));
			if(PrimaryClientId == clientId) {
				if(SecondaryClientIds.Count > 0) {
					PrimaryClientId = SecondaryClientIds[0];
					SecondaryClientIds.RemoveAt(0);
					ChangeRole(PrimaryClientId, true);
				} else {
					PrimaryClientId = null;
				}
			} else {
				SecondaryClientIds.Remove(clientId);
			}
		}

		private void ChangeRole(string clientId, bool isPrimary) {
			if(RoleChanges.TryGetValue(clientId, out bool isPrimaryExisting)) {
				if(isPrimaryExisting != isPrimary)
					RoleChanges[clientId] = isPrimary;
			} else
				RoleChanges.Add(clientId, isPrimary);
		}

		public void SwapPrimary() {
			if(SecondaryClientIds.Count == 0)
				return;
			string prevPrimary = PrimaryClientId;
			PrimaryClientId = SecondaryClientIds[0];
			SecondaryClientIds.RemoveAt(0);
			SecondaryClientIds.Add(prevPrimary);
			ChangeRole(prevPrimary, false);
			ChangeRole(PrimaryClientId, true);
		}

		public bool? AcknowledgeRole(string clientId) {
			if(RoleChanges.TryGetValue(clientId, out bool isPrimary)) {
				RoleChanges.Remove(clientId);
				return isPrimary;
			} else
				return null;
		}

		public bool IsPrimary(string clientId) => PrimaryClientId == clientId;

		public bool IsSecondary(string clientId) => SecondaryClientIds.Contains(clientId);

		public void Update(SessionInfo info, List<Standing> standings, DateTime timestamp) {
			LastInfo = info ?? LastInfo;
			LastStandings = standings ?? LastStandings;
			if(standings != null) {
				standings.Sort((a, b) => a.position.CompareTo(b.position));
				History.Update(standings, timestamp);
			}
			LastUpdate = timestamp;
			Finished = IsFinished(info);
		}

		public void Close() {
			Finished = true;
		}

		private bool IsFinished(SessionInfo info) {
			return Finished || (info != null && info.gamePhase == (int)GamePhase.Checkered);
		}

		public bool IsSameSession(SessionInfo info, MultiplayerTeams teams = null) {
			bool same = Track == info.trackName &&
				Type == info.session &&
				CompletionNotDecreased(info) &&
				IsValidPhaseTransition(info);
			if(!same || Online != (teams != null))
				return false;
			return Entries.IsSameEntryList(new EntryList(teams));
		}

		private bool CompletionNotDecreased(SessionInfo info) {
			if(LastInfo.raceCompletion == null || info.raceCompletion == null)
				return true;
			double last = LastInfo.raceCompletion.timeCompletion;
			double curr = info.raceCompletion.timeCompletion;
			return last == -1 || curr == -1 || last <= curr;
		}

		/// <summary>
		/// For this purpose, valid transitions are:<br/>
		///	&gt; no change<br/>
		///	&gt; advancing anywhere in starting sequence from starting to green (or a main phase)<br/>
		///	&gt; changing between any two main phases<br/>
		///	&gt; between checkered and paused but only between these two once checkered the first time<br/>
		///	&gt; between paused and anything else (technically this has potential to go backwards in starting sequence)<br/>
		/// </summary>
		private bool IsValidPhaseTransition(SessionInfo info) {
			return LastInfo.gamePhase == info.gamePhase ||
				(InStartingSequence(LastInfo.gamePhase) && LastInfo.gamePhase < info.gamePhase) ||
				(InMainPhase(LastInfo.gamePhase) && InMainPhase(info.gamePhase) && !Finished) ||
				(LastInfo.gamePhase == (int)GamePhase.Paused && info.gamePhase < (int)GamePhase.Checkered && !Finished) ||
				(LastInfo.gamePhase == (int)GamePhase.Checkered && info.gamePhase == (int)GamePhase.Paused) ||
				(LastInfo.gamePhase == (int)GamePhase.Paused && info.gamePhase == (int)GamePhase.Checkered);
		}

		private bool InStartingSequence(int phase) {
			return phase >= (int)GamePhase.Starting && phase <= (int)GamePhase.Green;
		}

		private bool InMainPhase(int phase) {
			return phase >= (int)GamePhase.Green && phase <= (int)GamePhase.Paused && phase != (int)GamePhase.Checkered;
		}

		public Session Clone() {
			Session session = Create(SessionId, LastInfo, Entries?.Reconstruct(), History.GetAllHistory().ConvertAll(x => x.Clone()));
			session.Update(LastInfo, LastStandings, LastUpdate);
			return session;
		}
	}
}
