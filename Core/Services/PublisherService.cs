using LMUSessionTracker.Core.Tracking;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public interface PublisherService {
		public Task Session(Session session);
	}
}
