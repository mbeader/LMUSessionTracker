using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class EntryList {
		public Dictionary<int, Entry> Slots { get; private set; }

		public EntryList(MultiplayerTeams teams = null) {
			Slots = new Dictionary<int, Entry>();
			if(teams != null)
				Populate(teams);
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
	}
}
