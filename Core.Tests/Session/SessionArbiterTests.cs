using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Session;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Session {
	public class SessionArbiterTests {
		private readonly Mock<ManagementRespository> managementRepo;
		private readonly SessionArbiter arbiter;
		private string clientId = "t";
		private byte sessionCount = 0;

		public SessionArbiterTests() {
			managementRepo = new Mock<ManagementRespository>();
			managementRepo.Setup(x => x.CreateSession(It.IsAny<SessionInfo>())).ReturnsAsync(() => new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ++sessionCount));
			arbiter = new SessionArbiter(Mock.Of<ILogger<SessionArbiter>>(), managementRepo.Object);
		}

		public static string SessionId(byte sessionId) => $"000000000000000000000000000000{sessionId:x2}";

		private static class Status {
			public static ProtocolStatus Rejected() => new() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null };
			public static ProtocolStatus ChangedPrimary(string sessionId) => new() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static ProtocolStatus ChangedSecondary(string sessionId) => new() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus AcceptedPrimary(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static ProtocolStatus AcceptedSecondary(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus Promoted(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus Demoted(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus ChangedPrimary(byte sessionId) => ChangedPrimary(SessionId(sessionId));
			public static ProtocolStatus ChangedSecondary(byte sessionId) => ChangedSecondary(SessionId(sessionId));
			public static ProtocolStatus AcceptedPrimary(byte sessionId) => AcceptedPrimary(SessionId(sessionId));
			public static ProtocolStatus AcceptedSecondary(byte sessionId) => AcceptedSecondary(SessionId(sessionId));
			public static ProtocolStatus Promoted(byte sessionId) => Promoted(SessionId(sessionId));
			public static ProtocolStatus Demoted(byte sessionId) => Demoted(SessionId(sessionId));
		}

		[Fact]
		public async Task Receive_Null_Rejects() {
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(null));
		}

		[Fact]
		public async Task Receive_NoClientId_Rejects() {
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new()));
		}

		[Fact]
		public async Task Receive_NoData_Rejects() {
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId }));
		}

		[Fact]
		public async Task Receive_InvalidSessionData_Rejects() {
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new SessionInfo() }));
		}

		[Fact]
		public async Task Receive_OfflineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
		}

		[Fact]
		public async Task Receive_SuccessiveSameSessionOfflineData_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new SessionInfo() }));
		}

		[Fact]
		public async Task Receive_SuccessiveSameSessionPausedOfflineData_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new SessionInfo() { gamePhase = 9 } }));
		}

		[Fact]
		public async Task Receive_SuccessiveDifferentSessionOfflineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() { trackName = "Sebring", session = "QUALIFY1" } }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new SessionInfo() { trackName = "Sebring", session = "RACE1" } }));
		}

		[Fact]
		public async Task Receive_ExistingSessionNoDataOffline_Rejects() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = null }));
		}
	}
}
