using System.Collections.Generic;

namespace LMUSessionTracker.Core.LMU {
	public class MultiplayerTeams {
		public int coherenceId { get; set; }
		public Dictionary<string, MultiplayerDriver> drivers { get; set; }
		public Dictionary<string, MultiplayerTeam> teams { get; set; }
	}
}
