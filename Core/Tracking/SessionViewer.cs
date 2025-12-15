using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionViewer {
		private readonly SessionManager manager;

		public SessionInfo Info => manager.Info;
		public List<Standing> Standings => manager.Standings;
		public History History => manager.History;

		public SessionViewer(SessionManager manager) {
			this.manager = manager;
		}
	}
}
