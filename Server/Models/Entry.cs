using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Server.Models {
	public class Entry {
		[Key, Required]
		public long EntryId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }

		[Required]
		public int SlotId { get; set; }
		[Required]
		public string Id { get; set; }
		[Required]
		public string Number { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Vehicle { get; set; }
		
		[JsonIgnore]
		public Session Session { get; set; }
		[JsonIgnore]
		public Car Car { get; set; }
		public ICollection<Member> Members { get; } = new List<Member>();

		public void From(Core.Tracking.Entry entry) {
			SlotId = entry.SlotId;
			Id = entry.Id;
			Number = entry.Number;
			Name = entry.Name;
			Vehicle = entry.Vehicle;
		}

		public Core.Tracking.Entry To() {
			Core.Tracking.Entry entry = new Core.Tracking.Entry() {
				SlotId = SlotId,
				Id = Id,
				Number = Number,
				Name = Name,
				Vehicle = Vehicle
			};
			foreach(Member member in Members)
				entry.Members.Add(member.To());
			return entry;
		}

		public bool IsSameEntry(Entry otherEntry) {
			return SlotId == otherEntry.SlotId &&
				Id == otherEntry.Id &&
				Number == otherEntry.Number &&
				Name == otherEntry.Name &&
				Vehicle == otherEntry.Vehicle &&
				Members.All(member => {
					Member otherMember = otherEntry.Members.First(otherMember => member.Name == otherMember.Name);
					return otherMember != null && member.IsSameMember(otherMember);
				});
		}
	}
}
