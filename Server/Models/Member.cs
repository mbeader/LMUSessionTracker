using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Member {
		[Key, Required]
		public long MemberId { get; set; }
		[ForeignKey(nameof(Entry)), Required]
		public long EntryId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public Guid SessionId { get; set; }

		public string Name { get; set; }
		public string Badge { get; set; }
		public string Nationality { get; set; }
		public bool IsDriver { get; set; }
		public bool IsEngineer { get; set; }
		public bool IsAdmin { get; set; }

		public Entry Entry { get; set; }
		public Session Session { get; set; }
	}
}
