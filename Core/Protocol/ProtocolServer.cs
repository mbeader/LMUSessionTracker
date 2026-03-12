using LMUSessionTracker.Common.Protocol;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Protocol {
	public interface ProtocolServer {
		public Task<ProtocolStatus> Receive(ProtocolMessage data);
	}
}
