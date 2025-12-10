using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class MultiplayerDriver {
		public string badge { get; set; }
		public bool isConnected { get; set; }
		public string nationality { get; set; }
		public List<string> roles { get; set; }
		public string teamId { get; set; }
		public string teamName { get; set; }
		public string uniqueTeamId { get; set; }
	}
}
