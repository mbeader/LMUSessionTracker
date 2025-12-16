using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionArbiter : ProtocolServer {
		private readonly ILogger<SessionArbiter> logger;
		private readonly ManagementRespository managementRepo;
		private readonly DateTimeProvider dateTimeProvider;
		private readonly UuidVersion7Provider uuidProvider;
		private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
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
			try {
				await semaphore.WaitAsync();
				Client client;
				if(!clients.TryGetValue(data.ClientId, out client)) {
					client = new Client(data.ClientId);
					clients.Add(client.ClientId, client);
					logger.LogInformation($"New client connected: {client.ClientId}");
				}
				client.LastSeen = now;
				if(data.SessionId == null) {
					return await HandleNoSession(client, data, now);
				} else if(activeSessions.TryGetValue(data.SessionId, out Session session)) {
					return await HandleSession(client, data, now, session);
				} else {
					return HandleInvalid(client, data, now);
				}
			} catch(Exception e) {
				logger.LogError(e, $"Failed to process message from client {data.ClientId}");
			} finally {
				semaphore.Release();
			}
			return Reject();
		}

		private async Task<ProtocolStatus> HandleNoSession(Client client, ProtocolMessage data, DateTime now) {
			if(data.SessionInfo != null) {
				Session session = FindExistingSession(client, data) ?? await ChangeSession(client, data, now);
				return Change(session.SessionId, session.IsPrimary(client.ClientId));
			} else {
				logger.LogInformation($"Client has no data no session: {client.ClientId}");
				return Reject();
			}
		}

		private async Task<ProtocolStatus> HandleSession(Client client, ProtocolMessage data, DateTime now, Session session) {
			if(data.SessionInfo == null) {
				session.UnregisterClient(data.ClientId);
				client.LeaveSession();
				logger.LogInformation($"Client {client.ClientId} left session {session.SessionId}");
				return Reject();
			}

			if(data.SessionInfo.gamePhase == 9) {
				logger.LogInformation($"Client {client.ClientId} observed paused session {session.SessionId}");
				return Accept(data.SessionId, true);
			}

			if(!session.IsSameSession(data.SessionInfo, data.MultiplayerTeams)) {
				session.UnregisterClient(data.ClientId);
				client.LeaveSession();
				session = FindExistingSession(client, data) ?? await ChangeSession(client, data, now);
				return Change(session.SessionId, session.IsPrimary(client.ClientId));
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
				return Accept(data.SessionId, false);

			await managementRepo.UpdateSession(session.SessionId, data.SessionInfo, now);
			session.Update(data.SessionInfo, data.Standings, now);
			await managementRepo.UpdateLaps(session.SessionId, session.History.GetAllHistory());
			return Accept(data.SessionId, true);
		}

		private ProtocolStatus HandleInvalid(Client client, ProtocolMessage data, DateTime now) {
			logger.LogInformation($"Client {client.ClientId} has invalid session {data.SessionId}");
			return Reject();
		}

		private Session FindExistingSession(Client client, ProtocolMessage data) {
			foreach(string sessionId in activeSessions.Keys) {
				Session session = activeSessions[sessionId];
				if(session.Online && session.IsSameSession(data.SessionInfo, data.MultiplayerTeams)) {
					bool isPrimary = session.RegisterClient(client.ClientId);
					client.JoinSession(session, isPrimary);
					if(data.SessionId == null)
						logger.LogInformation($"Client {client.ClientId} joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
					else
						logger.LogInformation($"Client {client.ClientId} transitioned from session {data.SessionId} to session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
					return session;
				}
			}
			return null;
		}

		private async Task<Session> ChangeSession(Client client, ProtocolMessage data, DateTime now) {
			string sessionId = uuidProvider.CreateVersion7(now).ToString("N");
			await managementRepo.CreateSession(sessionId, data.SessionInfo, now);
			Session session = Session.Create(sessionId, data.SessionInfo, data.MultiplayerTeams);
			if(session.Online)
				await managementRepo.UpdateEntries(sessionId, session.Entries);
			activeSessions.Add(session.SessionId, session);
			bool isPrimary = session.RegisterClient(client.ClientId);
			logger.LogInformation($"New session created: {session.SessionId}");
			if(data.SessionId == null)
				logger.LogInformation($"Client {client.ClientId} joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
			else
				logger.LogInformation($"Client {client.ClientId} transitioned from session {data.SessionId} to session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
			return session;
		}

		private ProtocolStatus Reject() {
			return new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null };
		}

		private ProtocolStatus PromoteOrDemote(string sessionId, bool isPrimary) {
			return new ProtocolStatus() {
				Result = isPrimary ? ProtocolResult.Promoted : ProtocolResult.Demoted,
				Role = isPrimary ? ProtocolRole.Primary : ProtocolRole.Secondary,
				SessionId = sessionId
			};
		}

		private ProtocolStatus Change(string sessionId, bool isPrimary) {
			return new ProtocolStatus() {
				Result = ProtocolResult.Changed,
				Role = isPrimary ? ProtocolRole.Primary : ProtocolRole.Secondary,
				SessionId = sessionId
			};
		}

		private ProtocolStatus Accept(string sessionId, bool isPrimary) {
			return new ProtocolStatus() {
				Result = ProtocolResult.Accepted,
				Role = isPrimary ? ProtocolRole.Primary : ProtocolRole.Secondary,
				SessionId = sessionId
			};
		}

		public async Task<Session> CloneSession(string sessionId) {
			await semaphore.WaitAsync();
			Session clonedSession = null;
			if(activeSessions.TryGetValue(sessionId, out Session session)) {
				clonedSession = session.Clone();
			}
			semaphore.Release();
			return clonedSession;
		}
	}
}
