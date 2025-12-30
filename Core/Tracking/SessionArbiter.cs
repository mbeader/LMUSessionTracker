using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionArbiter : ProtocolServer {
		private static readonly TimeSpan pruneLimit = new TimeSpan(0, 20, 0);
		private static readonly TimeSpan activeLimit = new TimeSpan(0, 10, 0);
		private readonly ILogger<SessionArbiter> logger;
		private readonly ManagementRespository managementRepo;
		private readonly DateTimeProvider dateTimeProvider;
		private readonly UuidVersion7Provider uuidProvider;
		private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
		private readonly SortedDictionary<string, Session> activeSessions = new SortedDictionary<string, Session>();
		private readonly SortedDictionary<string, Session> inactiveSessions = new SortedDictionary<string, Session>();
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
				await Prune(now);
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
				return await JoinNewOrExistingSession(client, data, now);
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

			if(session.LastInfo?.gamePhase != data.SessionInfo.gamePhase)
				logger.LogDebug($"Client {client.ClientId} in session {session.SessionId} phase changed from {Format.Phase(session.LastInfo?.gamePhase)} to {Format.Phase(data.SessionInfo.gamePhase)}");

			if(data.SessionInfo.gamePhase == 9) {
				logger.LogInformation($"Client {client.ClientId} observed paused session {session.SessionId}");
				return Accept(data.SessionId, true);
			}

			if(!session.IsSameSession(data.SessionInfo, data.MultiplayerTeams).IsSame) {
				session.UnregisterClient(data.ClientId);
				client.LeaveSession();
				return await JoinNewOrExistingSession(client, data, now);
			} else if(client.CurrentSession == null) {
				bool isPrimary = session.RegisterClient(client.ClientId);
				client.JoinSession(session, isPrimary);
				logger.LogInformation($"Client {client.ClientId} directly joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
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
			if(session.Finished && !inactiveSessions.ContainsKey(session.SessionId)) {
				inactiveSessions.Add(session.SessionId, session);
				await managementRepo.CloseSession(session.SessionId);
				logger.LogInformation($"Session {session.SessionId} is pending closure");
			}
			return Accept(data.SessionId, true);
		}

		private ProtocolStatus HandleInvalid(Client client, ProtocolMessage data, DateTime now) {
			logger.LogInformation($"Client {client.ClientId} has invalid session {data.SessionId}");
			return Reject();
		}

		private async Task<ProtocolStatus> JoinNewOrExistingSession(Client client, ProtocolMessage data, DateTime now) {
			Session session = FindExistingSession(client, data);
			if(client.CurrentSession != null) {
				string oldSessionId = client.CurrentSession.SessionId;
				if(activeSessions.TryGetValue(oldSessionId, out Session oldSession)) {
					oldSession.UnregisterClient(data.ClientId);
				}
				client.LeaveSession();
				logger.LogInformation($"Client {client.ClientId} left session {oldSessionId} (stale)");
			}
			if(session == null)
				session = await ChangeSession(client, data, now);
			else if(session.Finished) {
				logger.LogInformation($"Client {client.ClientId} attempted to join closed session {session.SessionId}");
				return Reject();
			} else {
				bool isPrimary = session.RegisterClient(client.ClientId);
				client.JoinSession(session, isPrimary);
				if(inactiveSessions.Remove(session.SessionId)) {
					logger.LogInformation($"Session {session.SessionId} marked as active");
				}
				if(data.SessionId == null)
					logger.LogInformation($"Client {client.ClientId} joined session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
				else
					logger.LogInformation($"Client {client.ClientId} transitioned from session {data.SessionId} to session {session.SessionId} as {(isPrimary ? "primary" : "secondary")}");
			}
			return Change(session.SessionId, session.IsPrimary(client.ClientId));
		}

		private Session FindExistingSession(Client client, ProtocolMessage data) {
			List<SessionDiff> diffs = new List<SessionDiff>();
			Session matchedSession = null;
			foreach(string sessionId in activeSessions.Keys) {
				Session session = activeSessions[sessionId];
				SessionDiff diff = session.IsSameSession(data.SessionInfo, data.MultiplayerTeams);
				diffs.Add(diff);
				if(matchedSession == null && session.Online && diff.IsSame) {
					matchedSession = session;
					break;
				}
			}
			foreach(SessionDiff diff in diffs) {
				logger.LogDebug($"Client {client.ClientId} diff with session {diff.SessionId} was {diff.Difference}: [{diff.GetMessage()}]");
			}
			return matchedSession;
		}

		private async Task<Session> ChangeSession(Client client, ProtocolMessage data, DateTime now) {
			string sessionId = uuidProvider.CreateVersion7(now).ToString("N");
			await managementRepo.CreateSession(sessionId, data.SessionInfo, now);
			Session session = Session.Create(sessionId, data.SessionInfo, now, data.MultiplayerTeams);
			if(session.Online)
				await managementRepo.UpdateEntries(sessionId, session.Entries);
			activeSessions.Add(session.SessionId, session);
			bool isPrimary = session.RegisterClient(client.ClientId);
			client.JoinSession(session, isPrimary);
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

		private async Task Prune(DateTime now) {
			List<string> pendingSessionIds = new List<string>();
			foreach(string sessionId in inactiveSessions.Keys) {
				Session session = inactiveSessions[sessionId];
				if(now - session.LastUpdate > pruneLimit) {
					if(!session.Finished) {
						List<string> clientIds = session.Close();
						foreach(string clientId in clientIds) {
							clients[clientId].LeaveSession();
							logger.LogInformation($"Client {clientId} left session {session.SessionId} by force");
						}
						await managementRepo.CloseSession(session.SessionId);
						logger.LogInformation($"Closing session {sessionId}");
					}
					if(session.HasClient())
						throw new Exception($"Cannot prune session {sessionId} due to presence of clients");
					logger.LogInformation($"Pruning session {sessionId}");
					pendingSessionIds.Add(sessionId);
				}
			}
			foreach(string sessionId in pendingSessionIds) {
				activeSessions.Remove(sessionId);
				inactiveSessions.Remove(sessionId);
			}

			pendingSessionIds.Clear();
			foreach(string sessionId in activeSessions.Keys) {
				Session session = activeSessions[sessionId];
				if(!inactiveSessions.ContainsKey(sessionId) && now - session.LastUpdate > activeLimit) {
					logger.LogInformation($"Marking session {sessionId} as inactive");
					inactiveSessions.Add(sessionId, session);
				}
			}
		}

		public async Task<Session> CloneSession(string sessionId) {
			if(sessionId == null)
				return null;
			await semaphore.WaitAsync();
			Session clonedSession = null;
			if(activeSessions.TryGetValue(sessionId, out Session session)) {
				clonedSession = session.Clone();
			}
			semaphore.Release();
			return clonedSession;
		}

		public async Task<List<SessionSummary>> SummarizeSessions() {
			await semaphore.WaitAsync();
			List<SessionSummary> summaries = new List<SessionSummary>();
			foreach(string sessionId in activeSessions.Keys) {
				summaries.Add(activeSessions[sessionId].Summarize(!inactiveSessions.ContainsKey(sessionId)));
			}
			semaphore.Release();
			return summaries;
		}

		public async Task Load() {
			await semaphore.WaitAsync();
			if(activeSessions.Count > 0)
				logger.LogWarning($"Found {activeSessions.Count} sessions when loading state");
			if(clients.Count > 0)
				logger.LogWarning($"Found {clients.Count} clients when loading state");
			activeSessions.Clear();
			clients.Clear();
			foreach(Session session in await managementRepo.GetSessions()) {
				if(!session.Finished)
					activeSessions.Add(session.SessionId, await managementRepo.GetSession(session.SessionId));
			}
			logger.LogInformation($"Loaded {activeSessions.Count} sessions");
			semaphore.Release();
		}
	}
}
