using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionArbiter : ProtocolServer {
		private readonly ILogger<SessionArbiter> logger;
		private readonly ManagementRespository managementRepo;
		private readonly DateTimeProvider dateTimeProvider;
		private readonly UuidVersion7Provider uuidProvider;
		private readonly Dictionary<string, Session> activeSessions = new Dictionary<string, Session>();
		private readonly Dictionary<string, Client> clients = new Dictionary<string, Client>();

		public SessionArbiter(ILogger<SessionArbiter> logger, ManagementRespository managementRepo, DateTimeProvider dateTimeProvider, UuidVersion7Provider uuidProvider) {
			this.logger = logger;
			this.managementRepo = managementRepo;
			this.dateTimeProvider = dateTimeProvider;
			this.uuidProvider = uuidProvider;
		}

		public async Task<ProtocolStatus> Receive(ProtocolMessage data) {
			DateTime now = dateTimeProvider.UtcNow;
			if(data?.ClientId == null)
				return Reject();
			Client client;
			if(!clients.TryGetValue(data.ClientId, out client)) {
				client = new Client(data.ClientId);
				clients.Add(client.ClientId, client);
				logger.LogInformation($"New client connected: {client.ClientId}");
			}
			client.LastSeen = now;
			if(data.SessionId == null) {
				if(data.SessionInfo != null) {
					Session existingSession = FindExistingSession(client, data);
					if(existingSession != null) {
						return new ProtocolStatus() { Result = ProtocolResult.Changed, Role = existingSession.IsPrimary(client.ClientId) ? ProtocolRole.Primary : ProtocolRole.Secondary, SessionId = existingSession.SessionId };
					}
					string sessionId = uuidProvider.CreateVersion7(now).ToString("N");
					await managementRepo.CreateSession(sessionId, data.SessionInfo, now);
					Session session = Session.Create(sessionId, data.SessionInfo, data.MultiplayerTeams);
					if(session.Online)
						await managementRepo.UpdateEntries(sessionId, session.Entries);
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
				if(!session.IsSameSession(data.SessionInfo, data.MultiplayerTeams)) {
					session.UnregisterClient(data.ClientId);
					client.LeaveSession();
					string sessionId = uuidProvider.CreateVersion7(now).ToString("N");
					await managementRepo.CreateSession(sessionId, data.SessionInfo, now);
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
				await managementRepo.UpdateSession(session.SessionId, data.SessionInfo, now);
				session.Update(data.SessionInfo, data.Standings, now);
				await managementRepo.UpdateLaps(session.SessionId, session.History.GetAllHistory());
				return new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = data.SessionId };
			} else {
				logger.LogInformation($"Client {client.ClientId} has invalid session {data.SessionId}");
				return Reject();
			}
		}

		private Session FindExistingSession(Client client, ProtocolMessage data) {
			foreach(string sessionId in activeSessions.Keys) {
				Session session = activeSessions[sessionId];
				if(session.Online && session.IsSameSession(data.SessionInfo, data.MultiplayerTeams)) {
					bool isPrimary = session.RegisterClient(client.ClientId);
					client.JoinSession(session, isPrimary);
					logger.LogInformation($"Client {client.ClientId} joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
					return session;
				}
			}
			return null;
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

		public Session CloneSession(string sessionId) {
			if(activeSessions.TryGetValue(sessionId, out Session session)) {
				return session.Clone();
			}
			return null;
		}
	}
}
