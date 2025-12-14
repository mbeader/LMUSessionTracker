using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session {
	public class Session {
		public Guid Guid { get; private set; }
		public string SessionId { get; private set; }
		public string PrimaryClientId { get; private set; }
		public List<string> SecondaryClientIds { get; private set; }
		public string Track { get; private set; }
		public string Type { get; private set; }
		public bool Online { get; private set; }
		public Dictionary<string, bool> RoleChanges { get; private set; }

		public static Session Create(Guid sessionId, SessionInfo info) {
			return new Session() {
				Guid = sessionId,
				SessionId = sessionId.ToString("N"),
				SecondaryClientIds = new List<string>(),
				Track = info.trackName,
				Type = info.session,
				RoleChanges = new Dictionary<string, bool>()
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

		public bool IsSameSession() {
			return true;
		}
	}
}
