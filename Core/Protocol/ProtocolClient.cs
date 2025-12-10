using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Protocol {
	public interface ProtocolClient {
		public Task<ProtocolStatus> Send(ProtocolMessage data);
	}
}
