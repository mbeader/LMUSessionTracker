using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Services {
	public class ClientServiceTests {
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private static readonly string publicKey = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0NCk1Db3dCUVlESzJWd0F5RUFTZkR4YUxxV1IxUmMzaVY4ZGxUQVRONW80UmhISTN5YUxhT2RBOGk3OUpjPQ0KLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0tDQo=";
		private static readonly string privateKey = "LS0tLS1CRUdJTiBQUklWQVRFIEtFWS0tLS0tDQpNQzRDQVFBd0JRWURLMlZ3QkNJRUlMNTZOVk5Sa2dFdGxUbS9sY0lmK1FsaWM3YlAySEc2dVVSQUNsekZPdnQ4DQotLS0tLUVORCBQUklWQVRFIEtFWS0tLS0tDQo=";
		private static readonly ClientId clientId;

		static ClientServiceTests() {
			clientId = ClientId.Import(Convert.FromBase64String(privateKey));
		}

		private readonly LoggingFixture loggingFixture;
		private readonly Mock<LMUClient> lmuClient;
		private readonly Mock<ProtocolClient> protocolClient;
		private readonly Mock<ClientHandler> handler;
		private readonly Mock<ClientHandlerFactory> handlerFactory;
		private readonly Mock<DateTimeProvider> dateTime;
		private readonly Mock<IServiceScope> scope;
		private readonly ClientService service;

		public ClientServiceTests(LoggingFixture loggingFixture) {
			this.loggingFixture = loggingFixture;
			ClientInfo clientInfo = new ClientInfo() { ClientId = clientId };
			lmuClient = new Mock<LMUClient>();
			protocolClient = new Mock<ProtocolClient>();
			handler = new Mock<ClientHandler>();
			handler.Setup(x => x.ClientId).Returns(clientInfo.ClientId.Hash);
			handlerFactory = new Mock<ClientHandlerFactory>();
			handlerFactory.Setup(x => x.Create(It.IsAny<LMUClient>(), It.IsAny<ProtocolClient>(), It.IsAny<ClientInfo>())).Returns(handler.Object);
			dateTime = new Mock<DateTimeProvider>();
			dateTime.Setup(x => x.UtcNow).Returns(baseTimestamp);
			scope = new Mock<IServiceScope>();
			scope.Setup(x => x.ServiceProvider.GetService(typeof(LMUClient))).Returns(lmuClient.Object);
			scope.Setup(x => x.ServiceProvider.GetService(typeof(ProtocolClient))).Returns(protocolClient.Object);
			scope.Setup(x => x.ServiceProvider.GetService(typeof(ClientHandlerFactory))).Returns(handlerFactory.Object);
			service = CreateService(clientInfo);
		}

		private ClientService CreateService(ClientInfo client) {
			Mock<IServiceScopeFactory> scopeFactory = new Mock<IServiceScopeFactory>();
			scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
			Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
			return new ClientService(loggingFixture.LoggerFactory.CreateLogger<ClientService>(), serviceProvider.Object, dateTime.Object, client);
		}

		private class TestState {
			public ClientState State { get; set; }
			public ProtocolRole Role { get; set; }
			public string SessionId { get; set; }

			public static TestState Idle() => new TestState() { State = ClientState.Idle, Role = ProtocolRole.None, SessionId = null };
			public static TestState OfflineWorking(string sessionId) => new TestState() { State = ClientState.Working, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static TestState OnlineWaiting(string sessionId) => new TestState() { State = ClientState.Connected, Role = ProtocolRole.None, SessionId = sessionId };
			public static TestState OnlineWorking(string sessionId) => new TestState() { State = ClientState.Working, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static TestState OnlineConnected(string sessionId) => new TestState() { State = ClientState.Connected, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static TestState Disconnected(string sessionId = null, ProtocolRole role = ProtocolRole.None) => new TestState() { State = ClientState.Disconnected, Role = role, SessionId = sessionId };
		}

		private void AssertState(TestState ex) {
			Assert.Equal(ex.State, service.State);
			Assert.Equal(ex.Role, service.Role);
			Assert.Equal(ex.SessionId, service.SessionId);
			Assert.Equal(clientId.Hash, service.ClientId);
		}

		[Fact]
		public async Task Start_InitialState_IsIdle() {
			await service.Start(scope.Object);
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Do_NoSession_IsIdle() {
			await service.Start(scope.Object);
			AssertState(TestState.Idle());
			await service.Do();
			AssertState(TestState.Idle());
		}
	}
}
