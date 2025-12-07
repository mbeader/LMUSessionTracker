using LMUSessionTracker.Core.Protocol;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Session {
	public class SessionArbiter : ProtocolServer {
		public Task<ProtocolStatus> Receive(ProtocolMessage data) {
			throw new System.NotImplementedException();
		}
	}
}
