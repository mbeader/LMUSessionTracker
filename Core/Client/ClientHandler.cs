using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Client {
	public class ClientHandler {
		private readonly ClientInfo client;
		private readonly LMUClient lmuClient;
		private readonly ProtocolClient protocolClient;
		private ClientState state = ClientState.Idle;
		private ProtocolRole role = ProtocolRole.None;
		private string sessionId;

		public ClientState State => state;
		public ProtocolRole Role => role;
		public string SessionId => sessionId;
		public string ClientId => client.ClientId;

		public ClientHandler(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client) {
			this.lmuClient = lmuClient;
			this.protocolClient = protocolClient;
			this.client = client;
		}

		public async Task HandleSession() {
			ProtocolMessage message = new ProtocolMessage() { ClientId = client.ClientId, SessionId = sessionId };
			message.SessionInfo = await lmuClient.GetSessionInfo();
			if(message.SessionInfo == null && state == ClientState.Idle) {
				// TODO temporarily gets all data for debugging
				await GetAllData(message);
				return;
			} else if(message.SessionInfo == null && state != ClientState.Idle) {
				state = ClientState.Idle;
				role = ProtocolRole.None;
				sessionId = null;
				await protocolClient.Send(message);
				// TODO temporarily gets all data for debugging
				await GetAllData(message);
				return;
			}
			await HandleActiveSession(message);
		}

		private async Task GetAllData(ProtocolMessage message) {
			List<Task> tasks = new List<Task>() {
				Task.Run(async () => { message.MultiplayerTeams = await lmuClient.GetMultiplayerTeams(); }),
				Task.Run(async () => { message.Standings = await lmuClient.GetStandings(); }),
				Task.Run(async () => { message.TeamStrategy = await lmuClient.GetStrategy(); }),
				Task.Run(async () => { message.Chat = await lmuClient.GetChat(); }),
				Task.Run(async () => { await lmuClient.GetStrategyUsage(); }),
				Task.Run(async () => { await lmuClient.GetStandingsHistory(); }),
				Task.Run(async () => { await lmuClient.GetMultiplayerJoinState(); }),
				Task.Run(async () => { await lmuClient.GetGameState(); }),
				Task.Run(async () => { await lmuClient.GetSessionsInfoForEvent(); }),
			};
			await Task.WhenAll(tasks);
		}

		private async Task HandleActiveSession(ProtocolMessage message) {
			// TODO temporarily gets all data for debugging
			await GetAllData(message);
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
