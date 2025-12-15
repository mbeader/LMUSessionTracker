using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class Entry {
		public int SlotId { get; set; }
		public string Id { get; set; }
		public string Number { get; set; }
		public string Name { get; set; }
		public string Vehicle { get; set; }
		public List<Member> Members { get; } = new List<Member>();

		public Entry() { }

		public Entry(int slotId, MultiplayerTeam team) {
			SlotId = slotId;
			Id = string.IsNullOrEmpty(team.Id) ? null : team.Id;
			Number = string.IsNullOrEmpty(team.carNumber) ? null : team.carNumber;
			Name = string.IsNullOrEmpty(team.name) ? null : team.name;
			Vehicle = string.IsNullOrEmpty(team.vehicle) ? null : team.vehicle;
			foreach(string member in team.drivers.Keys) {
				Members.Add(new Member(member, team.drivers[member]));
			}
		}

		public bool IsSameEntry(Entry otherEntry) {
			return SlotId == otherEntry.SlotId &&
				Id == otherEntry.Id &&
				Number == otherEntry.Number &&
				Name == otherEntry.Name &&
				Vehicle == otherEntry.Vehicle &&
				Members.TrueForAll(member => {
					Member otherMember = otherEntry.Members.Find(otherMember => member.Name == otherMember.Name);
					return otherMember != null && member.IsSameMember(otherMember);
				});
		}
	}
}
