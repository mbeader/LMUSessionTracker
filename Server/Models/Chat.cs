using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	[Index(nameof(Timestamp), nameof(Message), IsUnique = true)]
	public class Chat {
		[Key, Required]
		public long ChatId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public Guid SessionId { get; set; }

		public string Message { get; set; }
		public DateTime Timestamp { get; set; }

		public Session Session { get; set; }
	}
}
