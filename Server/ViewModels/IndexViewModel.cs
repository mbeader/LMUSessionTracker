using LMUSessionTracker.Core.Tracking;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class IndexViewModel {
		public List<SessionSummary> Sessions { get; set; }
		public int Total { get; set; }
	}
}
