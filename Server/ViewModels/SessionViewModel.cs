using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Server.ViewModels {
	public class SessionViewModel {
		public SessionInfo Info { get; set; }
		public List<Standing> Standings { get; set; }
		public List<CarHistory> History { get; set; }
		public Dictionary<CarKey, int> PositionInClass { get; } = new Dictionary<CarKey, int>();
		public Dictionary<CarKey, Car> Entries { get; } = new Dictionary<CarKey, Car>();

		public Models.Session Session { get; set; }
		public Models.SessionState SessionState => Session.LastState;
	}
}
