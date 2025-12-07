using LMUSessionTracker.Core.Protocol;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public class HttpProtocolClient : ProtocolClient {

		public Task<ProtocolStatus> Send(ProtocolMessage data) {
			return Task.FromResult(new ProtocolStatus() { Role = ProtocolRole.None, Result = ProtocolResult.Rejected });
		}
	}
}
