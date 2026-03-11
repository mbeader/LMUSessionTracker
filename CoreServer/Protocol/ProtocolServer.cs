using LMUSessionTracker.Core.Protocol;
using System.Threading.Tasks;

namespace LMUSessionTracker.CoreServer.Protocol {
	public interface ProtocolServer {
		public Task<ProtocolStatus> Receive(ProtocolMessage data);
	}
}
