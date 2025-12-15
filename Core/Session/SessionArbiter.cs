using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Session {
	public class SessionArbiter : ProtocolServer {
		private readonly ILogger<SessionArbiter> logger;
		private readonly ManagementRespository managementRepo;
		private readonly Dictionary<string, Session> activeSessions = new Dictionary<string, Session>();
		private readonly Dictionary<string, Client> clients = new Dictionary<string, Client>();

		public SessionArbiter(ILogger<SessionArbiter> logger, ManagementRespository managementRepo) {
			this.logger = logger;
			this.managementRepo = managementRepo;
		}

		public async Task<ProtocolStatus> Receive(ProtocolMessage data) {
			if(data?.ClientId == null)
				return Reject();
			Client client;
			if(!clients.TryGetValue(data.ClientId, out client)) {
				client = new Client(data.ClientId);
				clients.Add(client.ClientId, client);
				logger.LogInformation($"New client connected: {client.ClientId}");
			}
			client.LastSeen = DateTime.UtcNow;
			if(data.SessionId == null) {
				if(data.SessionInfo != null) {
					Guid sessionId = (await managementRepo.CreateSession(data.SessionInfo));
					Session session = Session.Create(sessionId, data.SessionInfo);
					activeSessions.Add(session.SessionId, session);
					bool isPrimary = session.RegisterClient(client.ClientId);
					logger.LogInformation($"New session created: {session.SessionId}");
					logger.LogInformation($"Client {client.ClientId} joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
					return new ProtocolStatus() { Result = ProtocolResult.Changed, Role = isPrimary ? ProtocolRole.Primary : ProtocolRole.Secondary, SessionId = session.SessionId };
				} else {
					logger.LogInformation($"Client has no data no session: {client.ClientId}");
					return Reject();
				}
			} else if(activeSessions.TryGetValue(data.SessionId, out Session session)) {
				if(data.SessionInfo == null) {
					session.UnregisterClient(data.ClientId);
					client.LeaveSession();
					logger.LogInformation($"Client {client.ClientId} left session {session.SessionId}");
					return Reject();
				}
				if(data.SessionInfo.gamePhase == 9) {
					logger.LogInformation($"Client {client.ClientId} observed paused session {session.SessionId}");
					return new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = data.SessionId };
				}
				if(!session.IsSameSession(data.SessionInfo)) {
					session.UnregisterClient(data.ClientId);
					client.LeaveSession();
					Guid sessionId = (await managementRepo.CreateSession(data.SessionInfo));
					session = Session.Create(sessionId, data.SessionInfo);
					activeSessions.Add(session.SessionId, session);
					bool isPrimary = session.RegisterClient(client.ClientId);
					logger.LogInformation($"New session created: {session.SessionId}");
					logger.LogInformation($"Client {client.ClientId} transitioned from session {data.SessionId} to session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
					return new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = session.SessionId };
				}
				bool? isPrimaryChange = session.AcknowledgeRole(client.ClientId);
				if(isPrimaryChange.HasValue) {
					if(isPrimaryChange.Value)
						client.Promote();
					else
						client.Demote();
					logger.LogInformation($"Client {client.ClientId} in session {session.SessionId} changed to {(isPrimaryChange.Value ? "primary" : "secondary")}");
					return PromoteOrDemote(data.SessionId, isPrimaryChange.Value);
				}
				if(session.IsSecondary(client.ClientId))
					return new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = data.SessionId };
				await managementRepo.UpdateSession(session.Guid, data.SessionInfo);
				session.Update(data.SessionInfo);
				return new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = data.SessionId };
			} else {
				logger.LogInformation($"Client {client.ClientId} has invalid session {data.SessionId}");
				return Reject();
			}
		}

		private ProtocolStatus Reject() {
			return new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null };
		}

		private ProtocolStatus PromoteOrDemote(string sessionId, bool isPrimary) {
			return new ProtocolStatus() {
				Result = isPrimary ? ProtocolResult.Promoted : ProtocolResult.Demoted,
				Role = isPrimary ? ProtocolRole.Primary : ProtocolRole.Secondary,
				SessionId = sessionId };
		}
	}
}
