using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Member {
		[Key, Required]
		public long MemberId { get; set; }
		[ForeignKey(nameof(Entry)), Required]
		public long EntryId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }

		public string Name { get; set; }
		public string Badge { get; set; }
		public string Nationality { get; set; }
		public bool IsDriver { get; set; }
		public bool IsEngineer { get; set; }
		public bool IsAdmin { get; set; }

		public Entry Entry { get; set; }
		public Session Session { get; set; }

		public void From(Core.Tracking.Member member) {
			Name = member.Name;
			Badge = member.Badge;
			Nationality = member.Nationality;
			IsDriver = member.IsDriver;
			IsEngineer = member.IsEngineer;
			IsAdmin = member.IsAdmin;
		}

		public Core.Tracking.Member To() {
			return new Core.Tracking.Member() {
				Name = Name,
				Badge = Badge,
				Nationality = Nationality,
				IsDriver = IsDriver,
				IsEngineer = IsEngineer,
				IsAdmin = IsAdmin,
			};
		}

		public bool IsSameMember(Member otherMember) {
			return Name == otherMember.Name &&
				Badge == otherMember.Badge &&
				Nationality == otherMember.Nationality &&
				IsAdmin == otherMember.IsAdmin &&
				IsDriver == otherMember.IsDriver &&
				IsEngineer == otherMember.IsEngineer;
		}
	}
}
