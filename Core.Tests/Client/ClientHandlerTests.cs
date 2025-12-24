using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Client {
	public class ClientHandlerTests {
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private static readonly string publicKey = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0NCk1Db3dCUVlESzJWd0F5RUFTZkR4YUxxV1IxUmMzaVY4ZGxUQVRONW80UmhISTN5YUxhT2RBOGk3OUpjPQ0KLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0tDQo=";
		private static readonly string privateKey = "LS0tLS1CRUdJTiBQUklWQVRFIEtFWS0tLS0tDQpNQzRDQVFBd0JRWURLMlZ3QkNJRUlMNTZOVk5Sa2dFdGxUbS9sY0lmK1FsaWM3YlAySEc2dVVSQUNsekZPdnQ4DQotLS0tLUVORCBQUklWQVRFIEtFWS0tLS0tDQo=";
		private static readonly ClientId clientId;

		static ClientHandlerTests() {
			clientId = ClientId.Import(Convert.FromBase64String(privateKey));
		}

		private readonly LoggingFixture loggingFixture;
		private readonly Mock<LMUClient> lmuClient;
		private readonly Mock<ProtocolClient> protocolClient;
		private readonly DefaultClientHandler handler;

		public ClientHandlerTests(LoggingFixture loggingFixture) {
			this.loggingFixture = loggingFixture;
			ClientInfo clientInfo = new ClientInfo() { ClientId = clientId };
			lmuClient = new Mock<LMUClient>();
			protocolClient = new Mock<ProtocolClient>();
			handler = new DefaultClientHandler(loggingFixture.LoggerFactory.CreateLogger<DefaultClientHandler>(), lmuClient.Object, protocolClient.Object, clientInfo);
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
			Assert.Equal(ex.State, handler.State);
			Assert.Equal(ex.Role, handler.Role);
			Assert.Equal(ex.SessionId, handler.SessionId);
			Assert.Equal(clientId.Hash, handler.ClientId);
		}

		[Fact]
		public void Handle_InitialState_IsIdle() {
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_NoGameState_IsIdle() {
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_NoSession_IsIdle() {
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OfflineSessionNewFailure_IsRejected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OfflineSessionNew_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OfflineSession_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OfflineSessionChanged_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s2" });
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s2"));
		}

		[Fact]
		public async Task Handle_OfflineSessionClosed_IsIdle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync((SessionInfo)null);
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OfflineSessionRejected_IsIdle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OfflineSessionNewFailure_IsDisconnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync((ProtocolStatus)null);
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.Disconnected());
		}

		[Fact]
		public async Task Handle_OfflineSessionFailure_IsDisconnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync((ProtocolStatus)null);
			await handler.Handle();
			AssertState(TestState.Disconnected("s1", ProtocolRole.Primary));
		}

		[Fact]
		public async Task Handle_OfflineSessionReconnect_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync((ProtocolStatus)null);
			await handler.Handle();
			AssertState(TestState.Disconnected("s1", ProtocolRole.Primary));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OfflineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionNew_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionNew_IsConnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionNewNoTeams_IsIdle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync((MultiplayerTeams)null);
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OnlineSession_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSession_IsConnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionDemoted_IsConnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Demoted, Role = ProtocolRole.Secondary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionPromoted_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Promoted, Role = ProtocolRole.Primary, SessionId = "s1" });
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
		}

		[Fact]
		public async Task Handle_OnlineSessionPrimaryChanged_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s2" });
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s2"));
		}

		[Fact]
		public async Task Handle_OnlineSessionPrimaryChanged_IsConnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s2" });
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s2"));
		}

		[Fact]
		public async Task Handle_OnlineSessionPrimaryClosed_Idle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync((SessionInfo)null);
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.None, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OnlineSessionPrimaryRejected_Idle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OnlineSessionSecondaryChanged_IsWorking() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s2" });
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s2"));
		}

		[Fact]
		public async Task Handle_OnlineSessionSecondaryChanged_IsConnected() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s2" });
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s2"));
		}

		[Fact]
		public async Task Handle_OnlineSessionSecondaryClosed_Idle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync((SessionInfo)null);
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.None, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OnlineSessionSecondaryRejected_IsIdle() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineConnected("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null });
			await handler.Handle();
			AssertState(TestState.Idle());
		}

		[Fact]
		public async Task Handle_OnlineSessionInvalidState_Throws() {
			lmuClient.Setup(x => x.GetSessionInfo()).ReturnsAsync(new SessionInfo());
			lmuClient.Setup(x => x.GetMultiplayerJoinState()).ReturnsAsync("JOIN_JOINED_SERVER");
			lmuClient.Setup(x => x.GetGameState()).ReturnsAsync(new GameState() { MultiStintState = "MONITOR_MENU" });
			lmuClient.Setup(x => x.GetMultiplayerTeams()).ReturnsAsync(new MultiplayerTeams());
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = "s1" });
			AssertState(TestState.Idle());
			await handler.Handle();
			AssertState(TestState.OnlineWorking("s1"));
			protocolClient.Setup(x => x.Send(It.IsAny<ProtocolMessage>())).ReturnsAsync(new ProtocolStatus() { Result = ProtocolResult.Promoted, Role = ProtocolRole.Primary, SessionId = "s1" });
			await Assert.ThrowsAsync<Exception>(handler.Handle);
		}
	}
}
