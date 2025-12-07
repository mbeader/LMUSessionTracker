using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Services {
	public class ClientServiceTests {
		private readonly Mock<LMUClient> client;
		private readonly Mock<IServiceScope> scope;
		private readonly ClientService service;

		public ClientServiceTests() {
			client = new Mock<LMUClient>();
			scope = new Mock<IServiceScope>();
			scope.Setup(x => x.ServiceProvider.GetService(typeof(LMUClient))).Returns(client.Object);
			Mock<IServiceScopeFactory> scopeFactory = new Mock<IServiceScopeFactory>();
			scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
			Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
			service = new ClientService(Mock.Of<ILogger<ClientService>>(), serviceProvider.Object);
		}

		[Fact]
		public async Task Start_InitialState_IsIdle() {
			await service.Start(scope.Object);
			Assert.Equal(ClientService.ClientState.Idle, service.State);
		}

		[Fact]
		public async Task Do_NoSession_IsIdle() {
			await service.Start(scope.Object);
			await service.Do();
			Assert.Equal(ClientService.ClientState.Idle, service.State);
		}

		[Fact]
		public async Task Do_OfflineSession_IsWorking() {
			client.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			await service.Start(scope.Object);
			await service.Do();
			Assert.Equal(ClientService.ClientState.Working, service.State);
		}

		[Fact]
		public async Task Do_OnlineSession_IsConnected() {
			client.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			client.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			await service.Start(scope.Object);
			await service.Do();
			Assert.Equal(ClientService.ClientState.Connected, service.State);
		}
	}
}
