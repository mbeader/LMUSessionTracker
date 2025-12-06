using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class MultiplayerTeam {
		public string Id { get; set; }
		public string carNumber { get; set; }
		public Dictionary<string, MultiplayerTeamMember> drivers { get; set; }
		public string name { get; set; }
		public string vehicle { get; set; }
	}
}
