using System;

namespace LMUSessionTracker.Core.Tracking {
	public class Client {
		public string ClientId { get; private init; }
		public bool IsConnected { get; private set; }
		public bool IsActive { get; private set; }
		public Session CurrentSession { get; private set; }
		public Session LastSession { get; private set; }
		public DateTime LastSeen { get; set; }

		public Client(string clientId) {
			ClientId = clientId;
		}

		public void JoinSession(Session session, bool isPrimary) {
			if(session == null)
				throw new ArgumentNullException(nameof(session));
			LastSession = CurrentSession;
			CurrentSession = session;
			IsConnected = true;
			IsActive = isPrimary;
		}

		public void LeaveSession() {
			LastSession = CurrentSession ?? LastSession;
			CurrentSession = null;
			IsActive = false;
		}

		public void MarkDisconnected() {
			IsConnected = false;
			IsActive = false;
		}

		public void Promote() {
			IsActive = true;
		}

		public void Demote() {
			IsActive = false;
		}
	}
}
