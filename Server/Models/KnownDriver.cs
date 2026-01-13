using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMUSessionTracker.Server.Models {
	public class KnownDriver {
		[Key, Required]
		public long DriverId { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
