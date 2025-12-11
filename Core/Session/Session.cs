using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session {
	public class Session {
		public string SessionId { get; private set; }
		public List<string> ClientIds { get; private set; }
		public string Track { get; private set; }
		public string Type { get; private set; }
		public bool Online { get; private set; }

		public static Session Create(string sessionId, SessionInfo info) {
			return new Session() {
				SessionId = sessionId,
				ClientIds = new List<string>(),
				Track = info.trackName,
				Type = info.session
			};
		}
	}
}
