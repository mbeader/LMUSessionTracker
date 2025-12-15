using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		public Session Session { get; set; }
		public ICollection<Member> Members { get; } = new List<Member>();

		public void From(Core.Tracking.Entry entry) {
			SlotId = entry.SlotId;
			Id = entry.Id;
			Number = entry.Number;
			Name = entry.Name;
			Vehicle = entry.Vehicle;
		}
	}
}
