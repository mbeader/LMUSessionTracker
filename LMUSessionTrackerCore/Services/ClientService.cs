using LMUSessionTracker.Core.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ClientService : PeriodicService<ClientService> {
		private ClientState state = ClientState.Idle;
		private ILMUClient client;

		public ClientState State => state;

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
			client = scope.ServiceProvider.GetRequiredService<ILMUClient>();
			return Task.CompletedTask;
		}

		public override async Task Do() {
			var sessionInfo = await client.GetSessionInfo();
			var multiplayerTeams = await client.GetMultiplayerTeams();
			if(sessionInfo == null) {
				state = ClientState.Idle;
				return;
			}
			if(multiplayerTeams != null) {
				state = ClientState.Connected;
			} else {
				state = ClientState.Working;
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
