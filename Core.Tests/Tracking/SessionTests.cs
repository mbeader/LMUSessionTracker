using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class SessionTests {
		private static readonly string id1 = "00000000000000000000000000000001";
		private static readonly string id2 = "00000000000000000000000000000002";

		public SessionTests() {
		}

		[Fact]
		public void IsSameSession_OfflineIdentical_ReturnsTrue() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Session session = Session.Create(id1, info);
			Assert.True(session.IsSameSession(info));
		}

		[Fact]
		public void IsSameSession_OfflineHigherCompletion_ReturnsTrue() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.True(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.6 } }));
		}

		[Fact]
		public void IsSameSession_OfflineNullOrDefault_ReturnsTrue() {
			SessionInfo infoWithValue = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithNull = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithDefault = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Assert.True(Session.Create(id1, infoWithValue).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithValue).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithValue));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithValue));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithDefault));
		}

		[Fact]
		public void IsSameSession_OfflineDifferentTrack_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1" });
			Assert.False(session.IsSameSession(new() { trackName = "Fuji", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_OfflineDifferentType_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "QUALIFY1" });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_OfflineLowerCompletion_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.4 } }));
		}

		[Fact]
		public void IsSameSession_OnlineIdentical_ReturnsTrue() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			Assert.True(session.IsSameSession(info, teams));
		}

		[Fact]
		public void IsSameSession_OnlineDifferentEntryList_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			teams.teams["utid0"].name = "team2";
			Assert.False(session.IsSameSession(info, teams));
		}

		[Fact]
		public void IsSameSession_OnlineToOffline_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			Assert.False(session.IsSameSession(info));
		}

		[Fact]
		public void IsSameSession_OfflineToOnline_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info);
			Assert.False(session.IsSameSession(info, teams));
		}
	}
}
