using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class SessionTests {
		private static readonly Guid guid1 = new Guid("00000000-0000-0000-0000-000000000001");
		private static readonly Guid guid2 = new Guid("00000000-0000-0000-0000-000000000002");

		public SessionTests() {
		}

		[Fact]
		public void IsSameSession_Identical_ReturnsTrue() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Session session = Session.Create(guid1, info);
			Assert.True(session.IsSameSession(info));
		}

		[Fact]
		public void IsSameSession_HigherCompletion_ReturnsTrue() {
			Session session = Session.Create(guid1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.True(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.6 } }));
		}

		[Fact]
		public void IsSameSession_NullOrDefault_ReturnsTrue() {
			SessionInfo infoWithValue = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithNull = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithDefault = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Assert.True(Session.Create(guid1, infoWithValue).IsSameSession(infoWithNull));
			Assert.True(Session.Create(guid1, infoWithValue).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(guid1, infoWithNull).IsSameSession(infoWithValue));
			Assert.True(Session.Create(guid1, infoWithNull).IsSameSession(infoWithNull));
			Assert.True(Session.Create(guid1, infoWithNull).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(guid1, infoWithDefault).IsSameSession(infoWithValue));
			Assert.True(Session.Create(guid1, infoWithDefault).IsSameSession(infoWithNull));
			Assert.True(Session.Create(guid1, infoWithDefault).IsSameSession(infoWithDefault));
		}

		[Fact]
		public void IsSameSession_DifferentTrack_ReturnsFalse() {
			Session session = Session.Create(guid1, new() { trackName = "Sebring", session = "RACE1" });
			Assert.False(session.IsSameSession(new() { trackName = "Fuji", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_DifferentType_ReturnsFalse() {
			Session session = Session.Create(guid1, new() { trackName = "Sebring", session = "QUALIFY1" });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_LowerCompletion_ReturnsFalse() {
			Session session = Session.Create(guid1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.4 } }));
		}
	}
}
