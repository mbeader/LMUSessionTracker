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

		public static Session Create(string sessionId, SessionInfo info) {
			return new Session() {
				SessionId = sessionId,
				SecondaryClientIds = new List<string>(),
				Track = info.trackName,
				Type = info.session,
				RoleChanges = new Dictionary<string, bool>(),
				LastInfo = info
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

		public void Update(SessionInfo info, List<Standing> standings) {
			LastInfo = info;
			LastStandings = standings;
			standings?.Sort((a, b) => a.position.CompareTo(b.position));
		}

		public bool IsSameSession(SessionInfo info) {
			return Track == info.trackName &&
				Type == info.session &&
				CompletionNotDecreased(info);
		}

		private bool CompletionNotDecreased(SessionInfo info) {
			if(LastInfo.raceCompletion == null || info.raceCompletion == null)
				return true;
			double last = LastInfo.raceCompletion.timeCompletion;
			double curr = info.raceCompletion.timeCompletion;
			return last == -1 || curr == -1 || last <= curr;
		}

		public Session Clone() {
			Session session = Create(SessionId, LastInfo);
			session.Update(LastInfo, LastStandings);
			return session;
		}
	}
}
