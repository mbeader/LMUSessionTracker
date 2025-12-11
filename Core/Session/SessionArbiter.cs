using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Session {
	public class SessionArbiter : ProtocolServer {
		private readonly ILogger<SessionArbiter> logger;
		private readonly ManagementRespository managementRepo;
		private readonly Dictionary<string, Session> activeSessions = new Dictionary<string, Session>();

		public SessionArbiter(ILogger<SessionArbiter> logger, ManagementRespository managementRepo) {
			this.logger = logger;
			this.managementRepo = managementRepo;
		}

		public async Task<ProtocolStatus> Receive(ProtocolMessage data) {
			if(data.SessionId == null) {
				if(data.SessionInfo != null) {
					string sessionId = (await managementRepo.CreateSession(data.SessionInfo)).ToString("N");
					activeSessions.Add(sessionId, Session.Create(sessionId, data.SessionInfo));
					return new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = sessionId };
				} else
					return Reject();
			} else if(activeSessions.TryGetValue(data.SessionId, out Session session)) {
				return new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = data.SessionId };
			} else {
				return Reject();
			}
		}

		private ProtocolStatus Reject() {
			return new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null };
		}
	}
}
