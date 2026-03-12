using System.Threading.Tasks;

namespace LMUSessionTracker.Common.Protocol {
	public interface ProtocolClient {
		public Task<ProtocolStatus> Send(ProtocolMessage data);
	}
}
