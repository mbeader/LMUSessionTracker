using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Tests.Client {
	public class ClientSessionStateTests {
		private static readonly ClientId clientId = TestClientId.ClientId;

		private readonly LoggingFixture loggingFixture;
		private readonly DefaultClientSessionState state;

		public ClientSessionStateTests(LoggingFixture loggingFixture) {
			this.loggingFixture = loggingFixture;
			ClientInfo clientInfo = new ClientInfo() { ClientId = clientId };
			state = new DefaultClientSessionState(loggingFixture.LoggerFactory.CreateLogger<DefaultClientSessionState>(), clientInfo);
		}

		private Chat ChatA() => new() { timestamp = 1, message = "a" };
		private Chat ChatB() => new() { timestamp = 2, message = "b" };
		private Chat ChatC() => new() { timestamp = 3, message = "c" };

		private void Assert_Filter_ChatWithoutInitialState(ProtocolState remoteState1, List<Chat> chat2, ProtocolState remoteState2, List<Chat> ex) {
			List<Chat> chat1 = new List<Chat>() { ChatA() };
			state.SetState(new(), remoteState1);
			Assert.Equivalent(remoteState1, state.State);
			List<Chat> ac = state.Filter(chat2 ?? chat1);
			if(ex == null)
				Assert.Null(ac);
			else
				AssertHelpers.Equivalent(ex, ac);
			state.SetState(new(), remoteState2);
			Assert.Equivalent(remoteState2, state.State);
		}

		[Fact]
		public void Filter_ChatWithoutInitialState_ReturnsAllChats() {
			Assert_Filter_ChatWithoutInitialState(null, null, new() { Chat = ChatA() }, new() { ChatA() });
		}

		[Fact]
		public void Filter_ChatWithoutInitialStateChat_ReturnsAllChats() {
			Assert_Filter_ChatWithoutInitialState(new(), null, new() { Chat = ChatA() }, new() { ChatA() });
		}

		[Fact]
		public void Filter_ChatUnchanged_ReturnsNull() {
			Assert_Filter_ChatWithoutInitialState(new() { Chat = ChatA() }, null, new() { Chat = ChatA() }, null);
		}

		[Fact]
		public void Filter_ChatChanged_ReturnsNewChats() {
			Assert_Filter_ChatWithoutInitialState(new() { Chat = ChatA() }, new List<Chat>() { ChatA(), ChatB(), ChatC() }, new() { Chat = ChatC() }, new() { ChatB(), ChatC() });
		}

		[Fact]
		public void Filter_ChatChangedSameTimestamp_ReturnsNewChats() {
			Chat chatb = new() { timestamp = 1, message = "b" };
			Assert_Filter_ChatWithoutInitialState(new() { Chat = ChatA() }, new List<Chat>() { ChatA(), chatb }, new() { Chat = chatb }, new() { chatb });
		}

		[Fact]
		public void Filter_ChatRemoteStateAhead_ReturnsNull() {
			Assert_Filter_ChatWithoutInitialState(new() { Chat = ChatB() }, null, new() { Chat = ChatB() }, null);
		}

		private void Assert_Filter_Strategy(ProtocolState remoteState1, List<TeamStrategy> strategies2, ProtocolState remoteState2, List<TeamStrategy> ex) {
			List<TeamStrategy> strategies1 = new List<TeamStrategy>() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1" } } } };
			state.SetState(new(), remoteState1);
			Assert.Equivalent(remoteState1, state.State);
			Assert_Filter_Strategy(strategies2 ?? strategies1, remoteState2, ex);
		}

		private void Assert_Filter_Strategy(List<TeamStrategy> strategies2, ProtocolState remoteState2, List<TeamStrategy> ex) {
			List<TeamStrategy> ac = state.Filter(strategies2);
			if(ex == null)
				Assert.Null(ac);
			else
				AssertHelpers.Equivalent(ex, ac);
			state.SetState(new(), remoteState2);
			Assert.Equivalent(remoteState2, state.State);
		}

		[Fact]
		public void Filter_StrategyWithoutInitialState_ReturnsAllStrategy() {
			Assert_Filter_Strategy(null, null, new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } },
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1" } } } });
		}

		[Fact]
		public void Filter_StrategyWithoutInitialStateChat_ReturnsAllStrategy() {
			Assert_Filter_Strategy(new(), null, new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } },
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1" } } } });
		}

		[Fact]
		public void Filter_StrategyUnchanged_ReturnsEmpty() {
			Assert_Filter_Strategy(new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } }, null,
				new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } },
				new());
		}

		[Fact]
		public void Filter_StrategyChanged_ReturnsNewStrategy() {
			Assert_Filter_Strategy(new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = 0 } } },
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1" }, new() { lap = 1, driver = "d1", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = 1 } } },
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 1, driver = "d1", tyres = new() { fl = new() { compound = "Medium" } } } } } });
		}

		[Fact]
		public void Filter_StrategyRemoteStateAhead_ReturnsEmpty() {
			Assert_Filter_Strategy(new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = 1 } } }, null,
				new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = 1 } } },
				new());
		}

		[Fact]
		public void Filter_StrategyTeamNotFound_ReturnsEmpty() {
			Assert_Filter_Strategy(new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } },
				new() { new() { Name = "t2", Strategy = new() { new() { lap = 0, driver = "d1" }, new() { lap = 1, driver = "d1", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 }, new() { Team = "t2", Driver = "d1", LastResolvedPitLap = 0 } } },
				new());
		}

		[Fact]
		public void Filter_StrategyDriverNotFound_ReturnsEmpty() {
			Assert_Filter_Strategy(new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 } } },
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0 }, new() { lap = 1, driver = "d2", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 }, new() { Team = "t2", Driver = "d1", LastResolvedPitLap = 0 } } },
				new());
		}

		private MultiplayerTeams MultiplayerTeams() {
			return new() {
				teams = new() {
					{ "utid0", new() { name = "t1", drivers = new() { { "d1", new() }, { "d2", new() } } } },
					{ "utid1", new() { name = "t2", drivers = new() { { "d2", new() } } } },
					{ "utid2", new() { name = "t1", drivers = new() { { "d3", new() } } } },
				}
			};
		}

		[Fact]
		public void Filter_StrategyOnlineDuplicateTeamNameFirstMatch_ReturnsStrategy() {
			ProtocolState rs = new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 }, new() { Team = "t1", Driver = "d3", LastResolvedPitLap = 2 } } };
			state.SetState(new() { MultiplayerTeams = MultiplayerTeams() }, rs);
			Assert_Filter_Strategy(
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				rs,
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 0, driver = "d1", tyres = new() { fl = new() { compound = "Medium" } } } } } });
		}

		[Fact]
		public void Filter_StrategyOnlineDuplicateTeamNameLastMatch_ReturnsStrategy() {
			ProtocolState rs = new() { Cars = new() { new() { Team = "t1", Driver = "d1", LastResolvedPitLap = -1 }, new() { Team = "t1", Driver = "d3", LastResolvedPitLap = 2 } } };
			state.SetState(new() { MultiplayerTeams = MultiplayerTeams() }, rs);
			Assert_Filter_Strategy(
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 4, driver = "d3", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				rs,
				new() { new() { Name = "t1", Strategy = new() { new() { lap = 4, driver = "d3", tyres = new() { fl = new() { compound = "Medium" } } } } } });
		}

		[Fact]
		public void Filter_StrategyOnlineUniqueTeamName_ReturnsStrategy() {
			ProtocolState rs = new() { Cars = new() { new() { Team = "t2", Driver = "d2", LastResolvedPitLap = -1 }, new() { Team = "t1", Driver = "d3", LastResolvedPitLap = 2 } } };
			state.SetState(new() { MultiplayerTeams = MultiplayerTeams() }, rs);
			Assert_Filter_Strategy(
				new() { new() { Name = "t2", Strategy = new() { new() { lap = 2, driver = "d2", tyres = new() { fl = new() { compound = "Medium" } } } } } },
				rs,
				new() { new() { Name = "t2", Strategy = new() { new() { lap = 2, driver = "d2", tyres = new() { fl = new() { compound = "Medium" } } } } } });
		}
	}
}
