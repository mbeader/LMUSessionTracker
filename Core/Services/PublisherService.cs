using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public interface PublisherService {
		public Task Session(Session session);
		public Task Prune(DateTime now, ICollection<string> sessionIds);
	}
}
