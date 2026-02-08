using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public interface TrackMapBuilder {
		public void Update(string sessionId, string track, List<Standing> standings);
	}
}
