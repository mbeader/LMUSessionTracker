using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Entry {
		[Key, Required]
		public long EntryId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public Guid SessionId { get; set; }

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
	}
}
