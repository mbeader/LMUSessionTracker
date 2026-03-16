using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Common.Services {
	public class ClientService : PeriodicService<ClientService> {
		private readonly ClientInfo client;
		private readonly IntervalProvider interval;
		private LMUClient lmuClient;
		private ProtocolClient protocolClient;
		private ClientIntervalProvider clientInterval;
		private ClientSessionState sessionState;
		private ContinueProvider<ClientService> continueProvider;
		private ClientHandler handler;

		public ClientState State => handler.State;
		public ProtocolRole Role => handler.Role;
		public string SessionId => handler.SessionId;
		public string ClientId => handler.ClientId;

		public ClientService(ILogger<ClientService> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime, ClientInfo client,
			IntervalProvider interval) : base(logger, serviceProvider, dateTime) {
			this.client = client;
			this.interval = interval;
		}

		public override int GetInterval() {
			return interval.GetInterval();
		}

		public override Task Start(IServiceScope scope) {
			ClientHandlerFactory handlerFactory = scope.ServiceProvider.GetRequiredService<ClientHandlerFactory>();
			lmuClient = scope.ServiceProvider.GetRequiredService<LMUClient>();
			protocolClient = scope.ServiceProvider.GetRequiredService<ProtocolClient>();
			clientInterval = scope.ServiceProvider.GetRequiredService<ClientIntervalProvider>();
			sessionState = scope.ServiceProvider.GetRequiredService<ClientSessionState>();
			continueProvider = scope.ServiceProvider.GetService<ContinueProvider<ClientService>>();
			handler = handlerFactory.Create(lmuClient, protocolClient, client, clientInterval, sessionState);
			logger.LogInformation($"Starting client as {handler.ClientId}");
			return Task.CompletedTask;
		}

		public override async Task<bool> Do() {
			(ClientState state, ProtocolRole role, string sessionId) last = (handler.State, handler.Role, handler.SessionId);
			DateTime timestamp = lmuClient.OpenContext();
			try {
				await handler.Handle(timestamp);
			} catch(Exception e) {
				logger.LogWarning(e, "Client run failed");
				handler.Reset();
			} finally {
				lmuClient.CloseContext();
			}
			if(last.state != handler.State || last.role != handler.Role || last.sessionId != handler.SessionId)
				logger.LogInformation($"State changed from ({last.state}, {last.role}, {last.sessionId}) to ({handler.State}, {handler.Role}, {handler.SessionId})");
			return continueProvider?.ShouldContinue() ?? true;
		}

		public override Task End() {
			return Task.CompletedTask;
		}
	}
}
