using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Client {
	public interface ClientHandler {
		public ClientState State { get; }
		public ProtocolRole Role { get; }
		public string SessionId { get; }
		public string ClientId { get; }

		public Task Handle();
	}

	public class DefaultClientHandler : ClientHandler {
		private readonly ILogger<DefaultClientHandler> logger;
		private readonly ClientInfo client;
		private readonly LMUClient lmuClient;
		private readonly ProtocolClient protocolClient;
		private ClientState state = ClientState.Idle;
		private ProtocolRole role = ProtocolRole.None;
		private string sessionId;
		private string lastMultiplayerJoinState;
		private string lastMultiStintState;
		private string lastLongState;
		private int lastLongStateCount;

		public ClientState State => state;
		public ProtocolRole Role => role;
		public string SessionId => sessionId;
		public string ClientId => client.ClientId.Hash;

		public DefaultClientHandler(ILogger<DefaultClientHandler> logger, LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client) {
			this.logger = logger;
			this.lmuClient = lmuClient;
			this.protocolClient = protocolClient;
			this.client = client;
		}

		public async Task Handle() {
			ProtocolMessage message = new ProtocolMessage() { ClientId = client.ClientId.Hash, SessionId = sessionId };
			await GetMainData(message);

			if(lastMultiplayerJoinState != message.MultiplayerJoinState || lastMultiStintState != message.GameState?.MultiStintState) {
				logger.LogInformation($"Game state changed from ({lastMultiplayerJoinState}, {lastMultiStintState}) to ({message.MultiplayerJoinState}, {message.GameState?.MultiStintState})");
				lastMultiplayerJoinState = message.MultiplayerJoinState;
				lastMultiStintState = message.GameState?.MultiStintState;
			}

			if(client.DebugMode) {
				string currLongState = $"{(message.SessionInfo != null ? "S" : " ")}{(message.MultiplayerJoinState != null ? "M" : " ")}{(message.GameState != null ? "G" : " ")} " +
				$"{message.SessionInfo?.session} {message.MultiplayerJoinState} {message.GameState?.MultiStintState}";
				if(lastLongState != currLongState) {
					logger.LogDebug($"{(lastLongStateCount > 999 ? 999 : lastLongStateCount):000} {currLongState}");
					lastLongState = currLongState;
					lastLongStateCount = 1;
				} else
					lastLongStateCount++;
			}

			if(message.GameState == null || (message.SessionInfo == null && state == ClientState.Idle)) {
				if(client.DebugMode)
					await GetAllData(message);
				return;
			} else if(message.SessionInfo == null && state != ClientState.Idle) {
				state = ClientState.Idle;
				role = ProtocolRole.None;
				sessionId = null;
				if(client.DebugMode)
					await GetAllData(message);
				await protocolClient.Send(message);
				return;
			}
			await HandleActiveSession(message);
		}

		private async Task GetMainData(ProtocolMessage message) {
			List<Task> tasks = new List<Task>() {
				Task.Run(async () => { message.SessionInfo = await lmuClient.GetSessionInfo(); }),
				Task.Run(async () => { message.MultiplayerJoinState = await lmuClient.GetMultiplayerJoinState(); }),
				Task.Run(async () => { message.GameState = await lmuClient.GetGameState(); }),
			};
			await Task.WhenAll(tasks);
		}

		private async Task GetAllData(ProtocolMessage message) {
			List<Task> tasks = new List<Task>() {
				Task.Run(async () => { message.MultiplayerTeams = await lmuClient.GetMultiplayerTeams(); }),
				Task.Run(async () => { message.Standings = await lmuClient.GetStandings(); }),
				Task.Run(async () => { await lmuClient.GetStrategy(); }),
				Task.Run(async () => { await lmuClient.GetChat(); }),
				Task.Run(async () => { await lmuClient.GetStrategyUsage(); }),
				Task.Run(async () => { await lmuClient.GetStandingsHistory(); }),
				Task.Run(async () => { await lmuClient.GetSessionsInfoForEvent(); }),
			};
			await Task.WhenAll(tasks);
		}

		private async Task HandleActiveSession(ProtocolMessage message) {
			if(client.DebugMode)
				await GetAllData(message);
			else
				message.MultiplayerTeams = await lmuClient.GetMultiplayerTeams();
			if(message.MultiplayerTeams == null && message.MultiplayerJoinState == "JOIN_JOINED_SERVER") {
				logger.LogDebug("Missing multiplayer teams");
				return;
			}
			switch(state) {
				case ClientState.Idle:
				case ClientState.Connected:
					break;
				case ClientState.Working:
					break;
				case ClientState.Disconnected:
					break;
				default:
					throw new Exception("Invalid pre-state");
			}
			ProtocolStatus result = await protocolClient.Send(message);
			if(result == null) {
				state = ClientState.Disconnected;
				return;
			}
			bool online = message.MultiplayerTeams != null;
			switch((state, result.Result, online)) {
				case (ClientState.Working, ProtocolResult.Accepted, true):
				case (ClientState.Connected, ProtocolResult.Accepted, true):
				case (ClientState.Working, ProtocolResult.Accepted, false):
					break;
				case (ClientState.Idle, ProtocolResult.Changed, true):
				case (ClientState.Idle, ProtocolResult.Changed, false):
				case (ClientState.Working, ProtocolResult.Changed, true):
				case (ClientState.Connected, ProtocolResult.Changed, true):
				case (ClientState.Working, ProtocolResult.Changed, false):
					role = result.Role;
					if(role == ProtocolRole.Primary)
						state = ClientState.Working;
					else
						state = ClientState.Connected;
					sessionId = result.SessionId;
					break;
				case (ClientState.Working, ProtocolResult.Demoted, true):
					role = result.Role;
					state = ClientState.Connected;
					break;
				case (ClientState.Connected, ProtocolResult.Promoted, true):
					role = result.Role;
					state = ClientState.Working;
					break;
				case (ClientState.Working, ProtocolResult.Rejected, true):
				case (ClientState.Connected, ProtocolResult.Rejected, true):
				case (ClientState.Working, ProtocolResult.Rejected, false):
					role = result.Role;
					state = ClientState.Idle;
					sessionId = null;
					break;
				case (ClientState.Disconnected, ProtocolResult.Accepted, true):
				case (ClientState.Disconnected, ProtocolResult.Changed, true):
				case (ClientState.Disconnected, ProtocolResult.Promoted, true):
				case (ClientState.Disconnected, ProtocolResult.Demoted, true):
				case (ClientState.Disconnected, ProtocolResult.Rejected, true):
				case (ClientState.Disconnected, ProtocolResult.Accepted, false):
				case (ClientState.Disconnected, ProtocolResult.Changed, false):
				case (ClientState.Disconnected, ProtocolResult.Promoted, false):
				case (ClientState.Disconnected, ProtocolResult.Demoted, false):
				case (ClientState.Disconnected, ProtocolResult.Rejected, false):
					role = result.Role;
					state = role == ProtocolRole.Primary ? ClientState.Working : ClientState.Connected;
					sessionId = result.SessionId;
					break;
				default:
					throw new Exception($"Invalid state. Online: {online}, Client: {state}, Result: {result.Result}");
			}
		}
	}
}
