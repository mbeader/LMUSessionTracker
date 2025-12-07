using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ClientService : PeriodicService<ClientService> {
		private LMUClient lmuClient;
		private ProtocolClient protocolClient;
		private ClientState state = ClientState.Idle;
		private ProtocolRole role = ProtocolRole.None;
		private string sessionId;
		private string clientId = "t";

		public ClientState State => state;
		public ProtocolRole Role => role;
		public string SessionId => sessionId;
		public string ClientId => clientId;

		public ClientService(ILogger<ClientService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider) {
		}

		public override int CalculateDelay() {
			switch(state) {
				case ClientState.Idle:
				case ClientState.Working:
					return 1000;
				case ClientState.Connected:
				case ClientState.Disconnected:
				default:
					return 10000;
			}
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<LMUClient>();
			protocolClient = scope.ServiceProvider.GetRequiredService<ProtocolClient>();
			return Task.CompletedTask;
		}

		public override async Task Do() {
			ProtocolMessage message = new ProtocolMessage() { ClientId = clientId, SessionId = sessionId };
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
			message.MultiplayerTeams = await lmuClient.GetMultiplayerTeams();
			if(message.MultiplayerTeams != null) {
				// online
				if(state == ClientState.Idle) {
					ProtocolStatus result = await protocolClient.Send(message);
					if(result == null) {
						state = ClientState.Disconnected;
					} else if(result.Result == ProtocolResult.Changed) {
						role = result.Role;
						if(role == ProtocolRole.Primary)
							state = ClientState.Working;
						else
							state = ClientState.Connected;
					}
					sessionId = result.SessionId;
				} else if(state == ClientState.Working) {
					// add other data
					ProtocolStatus result = await protocolClient.Send(message);
					if(result == null) {
						state = ClientState.Disconnected;
					} else if(result.Result == ProtocolResult.Changed) {
						sessionId = result.SessionId;
					} else if(result.Result == ProtocolResult.Demoted) {
						role = result.Role;
						state = ClientState.Connected;
					} else if(result.Result == ProtocolResult.Rejected) {
						role = result.Role;
						state = ClientState.Idle;
						sessionId = null;
					}
				} else if(state == ClientState.Connected) {
					ProtocolStatus result = await protocolClient.Send(message);
					if(result == null) {
						state = ClientState.Disconnected;
					} else if(result.Result == ProtocolResult.Changed) {
						sessionId = result.SessionId;
					} else if(result.Result == ProtocolResult.Promoted) {
						role = result.Role;
						state = ClientState.Working;
					} else if(result.Result == ProtocolResult.Rejected) {
						role = result.Role;
						state = ClientState.Idle;
						sessionId = null;
					}
				}
			} else {
				// offline
				if(state == ClientState.Idle) {
					ProtocolStatus result = await protocolClient.Send(message);
					if(result == null) {
						state = ClientState.Disconnected;
					} else if(result.Result == ProtocolResult.Changed) {
						role = result.Role;
						state = ClientState.Working;
					}
					sessionId = result.SessionId;
				} else if(state == ClientState.Working) {
					// add other data
					ProtocolStatus result = await protocolClient.Send(message);
					if(result == null) {
						state = ClientState.Disconnected;
					} else if(result.Result == ProtocolResult.Changed) {
						sessionId = result.SessionId;
					} else if(result.Result == ProtocolResult.Rejected) {
						role = result.Role;
						state = ClientState.Idle;
						sessionId = null;
					}
				}
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
