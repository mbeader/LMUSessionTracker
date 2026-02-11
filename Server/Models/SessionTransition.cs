using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Server.Models {
	public class SessionTransition {
		[Key, Required]
		public long SessionTransitionId { get; set; }
		[Required]
		public string FromSessionId { get; set; }
		[Required]
		public string ToSessionId { get; set; }

		[JsonIgnore]
		[InverseProperty(nameof(Session.FutureSessions))]
		public Session FromSession { get; set; }
		[JsonIgnore]
		[InverseProperty(nameof(Session.PastSessions))]
		public Session ToSession { get; set; }
	}
}
