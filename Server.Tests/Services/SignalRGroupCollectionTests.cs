using LMUSessionTracker.Server.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Tests.Services {
	public class SignalRGroupCollectionTests {
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private readonly SignalRGroupCollection service;

		public SignalRGroupCollectionTests(LoggingFixture loggingFixture) {
			service = new SignalRGroupCollection(loggingFixture.LoggerFactory.CreateLogger<SignalRGroupCollection>());
		}

		private void AssertGroupCollection(Dictionary<string, HashSet<string>> exGroups, Dictionary<string, HashSet<string>> exConnections) {
			Assert.Equivalent(exGroups.Keys, service.Groups, true);
			Assert.Equivalent(exConnections.Keys, service.Connections, true);
			foreach(string group in exGroups.Keys)
				Assert.Equivalent(exGroups[group], service.GetConnections(group), true);
			foreach(string connection in exConnections.Keys)
				Assert.Equivalent(exConnections[connection], service.GetGroups(connection), true);
		}

		[Fact]
		public void GetGroups_ForConnectionNoGroupsNoConnections_Empty() {
			Assert.Empty(service.GetGroups("c1"));
		}

		[Fact]
		public void GetConnections_ForGroupNoGroupsNoConnections_Empty() {
			Assert.Empty(service.GetConnections("g1"));
		}

		[Fact]
		public void AddOrUpdateGroup_NewGroup_OneGroupNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			AssertGroupCollection(new() { { "g1", new() } }, new());
		}

		[Fact]
		public void AddOrUpdateGroup_ExistingGroup_OneGroupNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g1", baseTimestamp);
			AssertGroupCollection(new() { { "g1", new() } }, new());
		}

		[Fact]
		public void RemoveGroup_NonexistingGroup_NoGroupsNoConnections() {
			service.RemoveGroup("g1");
			AssertGroupCollection(new(), new());
		}

		[Fact]
		public void RemoveGroup_ExistingGroup_NoGroupsNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.RemoveGroup("g1");
			AssertGroupCollection(new(), new());
		}

		[Fact]
		public void RemoveGroup_ExistingGroupWithConnection_NoGroupsNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.RemoveGroup("g1");
			AssertGroupCollection(new(), new());
		}

		[Fact]
		public void RemoveGroup_ExistingGroupWithConnectionWithOtherGroup_OneGroupOneConnection() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g2", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g2", "c1");
			service.RemoveGroup("g2");
			AssertGroupCollection(new() { { "g1", new() { "c1" } } }, new() { { "c1", new() { "g1" } } });
		}

		[Fact]
		public void AddConnectionToGroup_NonexistingGroup_Throws() {
			Assert.Throws<Exception>(() => service.AddConnectionToGroup("g1", "c1"));
		}

		[Fact]
		public void AddConnectionToGroup_ExistingGroup_OneGroupOneConnection() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			AssertGroupCollection(new() { { "g1", new() { "c1" } } }, new() { { "c1", new() { "g1" } } });
		}

		[Fact]
		public void AddConnectionToGroup_ExistingGroupExistingConnection_OneGroupOneConnection() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g1", "c1");
			AssertGroupCollection(new() { { "g1", new() { "c1" } } }, new() { { "c1", new() { "g1" } } });
		}

		[Fact]
		public void AddConnectionToGroup_ConnectionWithMultipleGroups_TwoGroupsOneConnection() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g2", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g2", "c1");
			AssertGroupCollection(new() { { "g1", new() { "c1" } }, { "g2", new() { "c1" } } }, new() { { "c1", new() { "g1", "g2" } } });
		}

		[Fact]
		public void AddConnectionToGroup_GroupWithMultipleConnections_OneGroupTwoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g1", "c2");
			AssertGroupCollection(new() { { "g1", new() { "c1", "c2" } } }, new() { { "c1", new() { "g1" } }, { "c2", new() { "g1" } } });
		}

		[Fact]
		public void RemoveConnectionFromAllGroups_ConnectionWithSingleGroup_OneGroupNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.RemoveConnectionFromAllGroups("c1");
			AssertGroupCollection(new() { { "g1", new() } }, new());
		}

		[Fact]
		public void RemoveConnectionFromAllGroups_ConnectionWithMultipleGroup_TwoGroupsNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g2", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g2", "c1");
			service.RemoveConnectionFromAllGroups("c1");
			AssertGroupCollection(new() { { "g1", new() }, { "g2", new() } }, new());
		}

		[Fact]
		public void RemoveConnectionFromAllGroups_GroupWithMultipleConnections_OneGroupOneConnection() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g1", "c2");
			service.RemoveConnectionFromAllGroups("c2");
			AssertGroupCollection(new() { { "g1", new() { "c1" } } }, new() { { "c1", new() { "g1" } } });
		}

		[Fact]
		public void SwapConnectionGroups_NonexistingGroups_Throws() {
			Assert.Throws<Exception>(() => service.SwapConnectionGroups("g1", "g2"));
		}

		[Fact]
		public void SwapConnectionGroups_GroupWithoutConnections_TwoGroupsNoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g2", baseTimestamp);
			Assert.Empty(service.SwapConnectionGroups("g1", "g2"));
			AssertGroupCollection(new() { { "g1", new() }, { "g2", new() } }, new());
		}

		[Fact]
		public void SwapConnectionGroups_GroupWithMultipleConnections_TwoGroupsTwoConnections() {
			service.AddOrUpdateGroup("g1", baseTimestamp);
			service.AddOrUpdateGroup("g2", baseTimestamp);
			service.AddConnectionToGroup("g1", "c1");
			service.AddConnectionToGroup("g1", "c2");
			Assert.Equivalent(new List<string>() { "c1", "c2" }, service.SwapConnectionGroups("g1", "g2"));
			AssertGroupCollection(new() { { "g1", new() }, { "g2", new() { "c1", "c2" } } }, new() { { "c1", new() { "g2" } }, { "c2", new() { "g2" } } });
		}
	}
}
