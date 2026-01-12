using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class EntryListTests {

		public EntryListTests() {
		}

		private Entry One() {
			return new Entry() {
				SlotId = 0,
				Id = "id1",
				Number = "1",
				Name = "team1",
				Vehicle = "veh1",
				Members = new List<Member>() {
					new Member() {
						Name = "driver1",
						Badge = "sr-saint",
						Nationality = "US",
						IsDriver = true
					}
				}
			};
		}

		private Entry Two() {
			return new Entry() {
				SlotId = 1,
				Id = "id2",
				Number = "2",
				Name = "team2",
				Vehicle = "veh2",
				Members = new List<Member>() {
					new Member() {
						Name = "driver2",
						Badge = "sr-noob",
						Nationality = "",
						IsDriver = true,
						IsEngineer = true
					}
				}
			};
		}

		private EntryList OneEntry() {
			return new EntryList(new List<Entry>() {
				One()
			});
		}

		private EntryList OneEntryTwo() {
			return new EntryList(new List<Entry>() {
				Two()
			});
		}

		private EntryList TwoEntries() {
			return new EntryList(new List<Entry>() {
				One(), Two()
			});
		}

		private MultiplayerTeams MultiplayerTeamsOneEntry() {
			return new MultiplayerTeams() {
				teams = new Dictionary<string, MultiplayerTeam>() {
					{ "utid0", new MultiplayerTeam() {
						Id = "id1",
						carNumber = "1",
						name = "team1",
						vehicle = "veh1",
						drivers = new Dictionary<string, MultiplayerTeamMember>() {
							{ "driver1", new MultiplayerTeamMember() {
								badge = "sr-saint",
								nationality = "US",
								roles = new List<string>() { "Driver" }
							} }
						}
					} }
				}
			};
		}

		[Fact]
		public void Construct_FromMultiplayerTeamsOneEntry() {
			Assert.Equivalent(OneEntry(), new EntryList(MultiplayerTeamsOneEntry()));
		}

		[Fact]
		public void Construct_FromMultiplayerTeamsOneEntryNullRoles() {
			EntryList ex = OneEntry();
			ex.Slots[0].Members[0].IsDriver = false;
			MultiplayerTeams teams = MultiplayerTeamsOneEntry();
			teams.teams["utid0"].drivers["driver1"].roles = null;
			Assert.Equivalent(ex, new EntryList(teams));
		}

		[Fact]
		public void Construct_FromMultiplayerTeamsOneEntryNullDrivers() {
			EntryList ex = OneEntry();
			ex.Slots[0].Members.Clear();
			MultiplayerTeams teams = MultiplayerTeamsOneEntry();
			teams.teams["utid0"].drivers = null;
			Assert.Equivalent(ex, new EntryList(teams));
		}

		[Fact]
		public void Construct_FromEntriesTwoEntries() {
			EntryList entries = new EntryList(new List<Entry>() { One(), Two() });
			Assert.Equivalent(new Dictionary<int, Entry>() { { 0, One() }, { 1, Two() } }, entries.Slots);
		}

		[Fact]
		public void Construct_FromEntriesOneChangedEntry() {
			Entry changed = One();
			changed.Name = "teama";
			EntryList entries = new EntryList(new List<Entry>() { One(), changed });
			Assert.Equivalent(new Dictionary<int, Entry>() { { 0, changed } }, entries.Slots);
			Assert.Equivalent(new List<(CarKey, Entry)>() { (new CarKey(0, "veh1"), One()) }, entries.Replaced);
		}

		[Fact]
		public void CalculateChange_ZeroToOne_ReturnsAdded() {
			Assert.Equal((0, 0, 0, 1), new EntryList().CalculateChange(OneEntry()));
		}

		[Fact]
		public void CalculateChange_OneToZero_ReturnsRemoved() {
			Assert.Equal((0, 1, 0, 0), OneEntry().CalculateChange(new EntryList()));
		}

		[Fact]
		public void CalculateChange_OneToSameOne_ReturnsSame() {
			Assert.Equal((1, 0, 0, 0), OneEntry().CalculateChange(OneEntry()));
		}

		[Fact]
		public void CalculateChange_OneToDifferentOne_ReturnsRemovedAndAdded() {
			Assert.Equal((0, 1, 0, 1), OneEntry().CalculateChange(OneEntryTwo()));
		}

		[Fact]
		public void CalculateChange_OneChanged_ReturnsChanged() {
			EntryList changed = OneEntry();
			changed.Slots.First().Value.Name = "teama";
			Assert.Equal((0, 0, 1, 0), OneEntry().CalculateChange(changed));
		}

		[Fact]
		public void CalculateChange_OneToTwo_ReturnsSameAndAdded() {
			Assert.Equal((1, 0, 0, 1), OneEntry().CalculateChange(TwoEntries()));
		}

		[Fact]
		public void CalculateChange_TwoToOne_ReturnsSameAndRemoved() {
			Assert.Equal((1, 1, 0, 0), TwoEntries().CalculateChange(OneEntry()));
		}

		[Fact]
		public void Merge_OneNoChanges_ReturnsFalse() {
			Assert.False(OneEntry().Merge(OneEntry()));
		}

		[Fact]
		public void Merge_TwoNoChanges_ReturnsFalse() {
			Assert.False(TwoEntries().Merge(TwoEntries()));
		}

		[Fact]
		public void Merge_OneToZero_ReturnsFalse() {
			Assert.False(TwoEntries().Merge(new EntryList()));
		}

		[Fact]
		public void Merge_ZeroToOne_ReturnsTrue() {
			Assert.True(new EntryList().Merge(OneEntry()));
		}

		[Fact]
		public void Merge_OneToDifferentOne_ReturnsTrue() {
			Assert.True(OneEntry().Merge(OneEntryTwo()));
		}

		[Fact]
		public void Merge_OneChanged_ReturnsTrue() {
			EntryList changed = OneEntry();
			changed.Slots.First().Value.Name = "teama";
			Assert.True(OneEntry().Merge(changed));
		}

		[Fact]
		public void Merge_OneToTwo_ReturnsTrue() {
			Assert.True(OneEntry().Merge(TwoEntries()));
		}

		[Fact]
		public void Merge_TwoToOne_ReturnsFalse() {
			Assert.False(TwoEntries().Merge(OneEntry()));
		}
	}
}
