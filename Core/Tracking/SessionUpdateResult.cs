using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionUpdateResult {
		public bool BestsChanged { get; init; }
		public bool EntrySlotsChanged { get; init; }
		public List<string> CarStateChanges { get; init; }
	}
}
