using LMUSessionTracker.Common.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.CoreServer.Tracking {
	public interface TrackMapBuilder {
		public void Update(string sessionId, string track, List<Standing> standings);
	}
}
