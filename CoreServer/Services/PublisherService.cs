using LMUSessionTracker.CoreServer.Tracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.CoreServer.Services {
	public interface PublisherService {
		public Task Session(Session session, bool includeBests);
		public Task Transition(Session session, string prevSessionId);
		public Task Sessions(List<SessionSummary> sessions);
		public Task Prune(DateTime now, ICollection<string> sessionIds);
	}
}
