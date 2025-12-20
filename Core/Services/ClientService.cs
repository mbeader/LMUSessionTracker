using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ClientService : PeriodicService<ClientService> {
		private readonly ClientInfo client;
		private LMUClient lmuClient;
		private ProtocolClient protocolClient;
		private ContinueProvider<ClientService> continueProvider;
		private ClientState state = ClientState.Idle;
		private ProtocolRole role = ProtocolRole.None;
		private string sessionId;

		public ClientState State => state;
		public ProtocolRole Role => role;
		public string SessionId => sessionId;
		public string ClientId => client.ClientId;

		public ClientService(ILogger<ClientService> logger, IServiceProvider serviceProvider, ClientInfo client) : base(logger, serviceProvider) {
			this.client = client;
		}

		public override int CalculateDelay() {
			switch(state) {
				case ClientState.Idle:
				case ClientState.Working:
					return 1000;
				case ClientState.Connected:
				case ClientState.Disconnected:
				default:
					return 1000;
			}
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<LMUClient>();
			protocolClient = scope.ServiceProvider.GetRequiredService<ProtocolClient>();
			continueProvider = scope.ServiceProvider.GetService<ContinueProvider<ClientService>>();
			return Task.CompletedTask;
		}

		public override async Task<bool> Do() {
			(ClientState state, ProtocolRole role, string sessionId) last = (state, role, sessionId);
			lmuClient.OpenContext();
			try {
				await HandleSession();
			} finally {
				lmuClient.CloseContext();
			}
			if(last.state != state || last.role != role || last.sessionId != sessionId)
				logger.LogInformation($"State changed from ({last.state}, {last.role}, {last.sessionId}) to ({state}, {role}, {sessionId})");
			return continueProvider?.ShouldContinue() ?? true;
		}

		private async Task HandleSession() {
			ProtocolMessage message = new ProtocolMessage() { ClientId = client.ClientId, SessionId = sessionId };
			message.SessionInfo = await lmuClient.GetSessionInfo();
			if(message.SessionInfo == null && state == ClientState.Idle) {
				return;
			} else if(message.SessionInfo == null && state != ClientState.Idle) {
				state = ClientState.Idle;
				role = ProtocolRole.None;
				sessionId = null;
				await protocolClient.Send(message);
				return;
			}
			await HandleActiveSession(message);
		}

		private async Task HandleActiveSession(ProtocolMessage message) {
			message.MultiplayerTeams = await lmuClient.GetMultiplayerTeams();
			switch(state) {
				case ClientState.Idle:
				case ClientState.Connected:
					break;
				case ClientState.Working:
					message.Standings = await lmuClient.GetStandings();
					message.TeamStrategy = await lmuClient.GetStrategy();
					message.Chat = await lmuClient.GetChat();
					break;
				case ClientState.Disconnected:
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
				default:
					throw new Exception($"Invalid state. Online: {online}, Client: {state}, Result: {result.Result}");
			}
		}

		public override Task End() {
			return Task.CompletedTask;
		}

		public enum ClientState {
			/// <summary>
			/// LMU is not in an active session, no data to send, not connected to ST server
			/// </summary>
			Idle,
			/// <summary>
			/// LMU is in an active session, connected to ST server but not primary data provider
			/// </summary>
			Connected,
			/// <summary>
			/// LMU is in an active session, not connected to ST server
			/// </summary>
			Disconnected,
			/// <summary>
			/// LMU is in an active session, connected to ST server as primary data provider
			/// </summary>
			Working
		}
	}
}
