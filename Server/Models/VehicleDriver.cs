using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	[PrimaryKey(nameof(Veh), nameof(Name))]
	public class VehicleDriver {
		[Required]
		public string Veh { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Nationality { get; set; }
		[Required]
		public string Skill { get; set; }

		[ForeignKey(nameof(Veh))]
		public Vehicle Vehicle { get; set; }
	}
}
