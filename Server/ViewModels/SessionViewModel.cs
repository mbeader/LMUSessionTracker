using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class SessionViewModel {
		public SessionInfo Info { get; set; }
		public List<Standing> Standings { get; set; }
		public List<CarHistory> History { get; set; }
		public Dictionary<CarKey, int> PositionInClass { get; } = new Dictionary<CarKey, int>();
	}
}
