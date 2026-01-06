using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class SessionArbiterTests {
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private readonly Mock<ManagementRespository> managementRepo;
		private readonly Mock<DateTimeProvider> dateTimeProvider;
		private readonly Mock<UuidVersion7Provider> uuidProvider;
		private readonly SessionArbiter arbiter;
		private string clientId = "t";
		private string clientI2 = "q";
		private byte sessionCount = 0;
		private DateTime lastTimestamp = baseTimestamp;

		public SessionArbiterTests(LoggingFixture loggingFixture) {
			managementRepo = new Mock<ManagementRespository>();
			dateTimeProvider = new Mock<DateTimeProvider>();
			dateTimeProvider.Setup(x => x.UtcNow).Returns(() => lastTimestamp).Callback(() => { lastTimestamp += new TimeSpan(0, 0, 1); });
			uuidProvider = new Mock<UuidVersion7Provider>();
			uuidProvider.Setup(x => x.CreateVersion7(It.IsAny<DateTime>())).Returns(() => Guid.Parse(SessionId(++sessionCount)));
			arbiter = new SessionArbiter(loggingFixture.LoggerFactory.CreateLogger<SessionArbiter>(), managementRepo.Object, dateTimeProvider.Object, uuidProvider.Object);
		}

		public static string SessionId(byte sessionId) => $"000000000000000000000000000000{sessionId:x2}";

		private static class Status {
			public static ProtocolStatus Rejected() => new() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null };
			public static ProtocolStatus ChangedPrimary(string sessionId) => new() { Result = ProtocolResult.Changed, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static ProtocolStatus ChangedSecondary(string sessionId) => new() { Result = ProtocolResult.Changed, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus AcceptedPrimary(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static ProtocolStatus AcceptedSecondary(string sessionId) => new() { Result = ProtocolResult.Accepted, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus Promoted(string sessionId) => new() { Result = ProtocolResult.Promoted, Role = ProtocolRole.Primary, SessionId = sessionId };
			public static ProtocolStatus Demoted(string sessionId) => new() { Result = ProtocolResult.Demoted, Role = ProtocolRole.Secondary, SessionId = sessionId };
			public static ProtocolStatus ChangedPrimary(byte sessionId) => ChangedPrimary(SessionId(sessionId));
			public static ProtocolStatus ChangedSecondary(byte sessionId) => ChangedSecondary(SessionId(sessionId));
			public static ProtocolStatus AcceptedPrimary(byte sessionId) => AcceptedPrimary(SessionId(sessionId));
			public static ProtocolStatus AcceptedSecondary(byte sessionId) => AcceptedSecondary(SessionId(sessionId));
			public static ProtocolStatus Promoted(byte sessionId) => Promoted(SessionId(sessionId));
			public static ProtocolStatus Demoted(byte sessionId) => Demoted(SessionId(sessionId));
		}

		[Fact]
		public async Task Load_FinishedSession_Closes() {
			Session session = Session.Create(SessionId(1), new() { gamePhase = (int)GamePhase.Checkered }, baseTimestamp, MultiplayerTeams());
			managementRepo.Setup(x => x.GetSessions()).ReturnsAsync(new List<Session>() { session });
			managementRepo.Setup(x => x.GetSession(SessionId(1))).ReturnsAsync(session);
			await arbiter.Load();
			managementRepo.Verify(x => x.CloseSession(SessionId(1)), Times.Once);
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
		public async Task Receive_Failure_Rejects() {
			managementRepo.Setup(x => x.CreateSession(It.IsAny<string>(), It.IsAny<SessionInfo>(), It.IsAny<DateTime>(), It.IsAny<bool>())).ThrowsAsync(new Exception("foo"));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
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

		[Fact]
		public async Task Receive_RepeatInitialOfflineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new SessionInfo() }));
		}

		private MultiplayerTeams MultiplayerTeams() {
			return new() {
				teams = new() {
					{ "utid0", new() {
						name = "team1",
						vehicle = "someveh",
						drivers = new() {
							{ "driver1", new() {
								roles = new() { "Driver" }
							} }
						}
					} }
				}
			};
		}

		[Fact]
		public async Task Receive_OnlineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_SuccessiveSameSessionOnlineData_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_SuccessiveSameSessionPausedOnlineData_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { gamePhase = 9 }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_SuccessiveDifferentSessionOnlineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "Sebring", session = "QUALIFY1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { trackName = "Sebring", session = "RACE1" }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_SuccessiveDifferentEntryListOnlineData_Creates() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			MultiplayerTeams teams2 = MultiplayerTeams();
			teams2.teams["utid0"].vehicle = "someotherveh";
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = teams2 }));
		}

		[Fact]
		public async Task Receive_ExistingSessionNoDataOnline_Rejects() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = null }));
		}

		[Fact]
		public async Task Receive_RejoinSessionOnlineData_ReregistersClient() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = null }));
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientOfflineData_CreatesBoth() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() }));
		}

		[Fact]
		public async Task Receive_MultiClientDifferentSessionOnlineData_CreatesBoth() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			MultiplayerTeams teams2 = MultiplayerTeams();
			teams2.teams["utid0"].vehicle = "someotherveh";
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new(), MultiplayerTeams = teams2 }));
		}

		[Fact]
		public async Task Receive_MultiClientSameSessionOnlineData_CreatesOne() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSuccessiveSameSessionOnlineData_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSameSessionOnlineSecondaryLeaves_PrimaryRemains() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(1), SessionInfo = null }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSameSessionOnlinePrimaryLeaves_PromotesSecondary() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = null }));
			Assert.Equivalent(Status.Promoted(1), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSameSessionOnlineWithinFuzziness_CreatesOne() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { timeRemainingInGamePhase = 55, raceCompletion = new() { timeCompletion = 0.45 } }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { timeRemainingInGamePhase = 60, raceCompletion = new() { timeCompletion = 0.40 } }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSessionTransitionPrimaryFirst_CreatesOne() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { session = "QUALIFY1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { session = "QUALIFY1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { session = "RACE1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(1), SessionInfo = new() { session = "RACE1" }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientSessionTransitionSecondaryFirst_CreatesOne() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { session = "QUALIFY1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(1), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { session = "QUALIFY1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(1), SessionInfo = new() { session = "RACE1" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedSecondary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { session = "RACE1" }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_MultiClientClosedSessionOnlineData_AcceptsExistingRejectsNew() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { gamePhase = 8 }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { gamePhase = 8 }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.Rejected(), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { gamePhase = 8 }, MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_SessionInactiveForLongerThanLimit_Prunes() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "a" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 5, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(2), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 10, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(2), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 15, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(2), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 20, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(2), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(2), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Null(await arbiter.CloneSession(SessionId(1)));
		}

		[Fact]
		public async Task Receive_SessionInactiveForLongerThanLimitWithExistingClient_Prunes() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "a" }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 20, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionId = SessionId(2), SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.Null(await arbiter.CloneSession(SessionId(1)));
		}

		[Fact]
		public async Task Receive_SessionInactiveForShorterThanLimit_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "a" }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 10, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.ChangedPrimary(2), await arbiter.Receive(new() { ClientId = clientI2, SessionInfo = new() { trackName = "b" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { trackName = "a" }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
		}

		[Fact]
		public async Task Receive_SessionInactiveForLongerThanLimitWhileStillInPhase_Accepts() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "a", timeRemainingInGamePhase = 30*60 }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 10, 1);
			await arbiter.Prune(lastTimestamp);
			lastTimestamp = baseTimestamp + new TimeSpan(0, 20, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new() { trackName = "a", timeRemainingInGamePhase = 10*60 }, MultiplayerTeams = MultiplayerTeams() }));
			Assert.NotNull(await arbiter.CloneSession(SessionId(1)));
		}

		[Fact]
		public async Task Receive_SessionInactiveForLongerThanLimitWhileStillInPhase_Prunes() {
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new() { trackName = "a", timeRemainingInGamePhase = 20*60 }, MultiplayerTeams = MultiplayerTeams() }));
			lastTimestamp = baseTimestamp + new TimeSpan(0, 10, 1);
			await arbiter.Prune(lastTimestamp);
			lastTimestamp = baseTimestamp + new TimeSpan(0, 20, 1);
			await arbiter.Prune(lastTimestamp);
			Assert.Null(await arbiter.CloneSession(SessionId(1)));
		}

		[Fact]
		public async Task Receive_JoinLoadedSession_Accepts() {
			Session session = Session.Create(SessionId(1), new(), baseTimestamp, MultiplayerTeams());
			managementRepo.Setup(x => x.GetSessions()).ReturnsAsync(new List<Session>() { session });
			managementRepo.Setup(x => x.GetSession(SessionId(1))).ReturnsAsync(session);
			await arbiter.Load();
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Fact]
		public async Task Receive_JoinReloadedSession_Accepts() {
			Session session = Session.Create(SessionId(1), new(), baseTimestamp, MultiplayerTeams());
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			managementRepo.Setup(x => x.GetSessions()).ReturnsAsync(new List<Session>() { session });
			managementRepo.Setup(x => x.GetSession(SessionId(1))).ReturnsAsync(session);
			await arbiter.Load();
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task Receive_JoinOldestLoadedSession_Accepts(bool ascending) {
			Session session1 = Session.Create(SessionId(1), new(), baseTimestamp, MultiplayerTeams());
			Session session2 = Session.Create(SessionId(2), new(), baseTimestamp.AddSeconds(1), MultiplayerTeams());
			managementRepo.Setup(x => x.GetSessions()).ReturnsAsync(new List<Session>() { ascending ? session1 : session2, ascending ? session2 : session1 });
			managementRepo.Setup(x => x.GetSession(SessionId(1))).ReturnsAsync(session1);
			managementRepo.Setup(x => x.GetSession(SessionId(2))).ReturnsAsync(session2);
			await arbiter.Load();
			Assert.Equivalent(Status.ChangedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
			Assert.Equivalent(Status.AcceptedPrimary(1), await arbiter.Receive(new() { ClientId = clientId, SessionId = SessionId(1), SessionInfo = new(), MultiplayerTeams = MultiplayerTeams() }));
		}
	}
}
