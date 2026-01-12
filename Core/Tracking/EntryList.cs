using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class EntryList {
		public Dictionary<int, Entry> Slots { get; private set; }
		public List<(CarKey, Entry)> Replaced { get; private set; }

		public EntryList(MultiplayerTeams teams = null) {
			Slots = new Dictionary<int, Entry>();
			Replaced = new List<(CarKey, Entry)>();
			if(teams != null)
				Populate(teams);
		}

		public EntryList(List<Entry> entries) {
			Slots = new Dictionary<int, Entry>();
			Replaced = new List<(CarKey, Entry)>();
			if(entries != null)
				foreach(Entry entry in entries) {
					if(Slots.TryGetValue(entry.SlotId, out Entry existingEntry)) {
						Replaced.Add((new CarKey(existingEntry.SlotId, existingEntry.Vehicle), existingEntry));
						Slots[entry.SlotId] = entry;
					} else
						Slots.Add(entry.SlotId, entry);
				}
		}

		private void Populate(MultiplayerTeams teams) {
			foreach(string team in teams.teams.Keys) {
				if(!team.StartsWith("utid"))
					throw new Exception("Unique team ID does not start with utid");
				int slotId = int.Parse(team.Substring(4));
				Entry entry = new Entry(slotId, teams.teams[team]);
				Slots.Add(slotId, entry);
			}
		}

		public MultiplayerTeams Reconstruct() {
			MultiplayerTeams teams = new MultiplayerTeams() { teams = new Dictionary<string, MultiplayerTeam>() };
			foreach(int slotId in Slots.Keys) {
				Entry entry = Slots[slotId];
				MultiplayerTeam team = new MultiplayerTeam() {
					Id = entry.Id,
					carNumber = entry.Number,
					name = entry.Name,
					vehicle = entry.Vehicle,
					drivers = new Dictionary<string, MultiplayerTeamMember>()
				};
				foreach(Member member in entry.Members) {
					MultiplayerTeamMember driver = new MultiplayerTeamMember() {
						badge = member.Badge,
						nationality = member.Nationality,
						roles = member.Roles()
					};
					team.drivers.Add(member.Name, driver);
				}
				teams.teams.Add($"utid{slotId}", team);
			}
			return teams;
		}

		public bool IsSameEntryList(EntryList other) {
			if(Slots.Count == other.Slots.Count) {
				foreach(int slotId in Slots.Keys) {
					if(other.Slots.TryGetValue(slotId, out Entry otherEntry)) {
						if(!Slots[slotId].IsSameEntry(otherEntry))
							return false;
					} else
						return false;
				}
				return true;
			} else
				return false;
		}

		public (int same, int removed, int changed, int added) CalculateChange(EntryList other) {
			int same = 0, removed = 0, changed = 0, added = 0;
			foreach(int slotId in Slots.Keys) {
				if(other.Slots.TryGetValue(slotId, out Entry otherEntry)) {
					if(!Slots[slotId].IsSameEntry(otherEntry))
						changed++;
					else
						same++;
				} else
					removed++;
			}
			added = other.Slots.Keys.Count - same - changed;
			return (same, removed, changed, added);
		}

		public bool HasAnyMatch(EntryList other) {
			(int same, int removed, int changed, int added) = CalculateChange(other);
			return same != 0 || (same + removed + changed + added == 0);
		}

		/// <summary>
		/// Returns whether the content of Slots changed
		/// </summary>
		public bool Merge(EntryList other) {
			bool changed = false;
			foreach(int slotId in other.Slots.Keys) {
				if(Slots.TryGetValue(slotId, out Entry existingEntry)) {
					if(!existingEntry.IsSameEntry(other.Slots[slotId])) {
						Replaced.Add((new CarKey(slotId, existingEntry.Vehicle), existingEntry));
						Slots[slotId] = other.Slots[slotId];
						changed = true;
					}
				} else {
					Slots.Add(slotId, other.Slots[slotId]);
					changed = true;
				}
			}
			return changed;
		}
	}
}
