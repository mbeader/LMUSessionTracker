using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Protocol {
	/// <summary>
	/// Rejects all incoming data
	/// </summary>
	public class AutoRejectServer : ProtocolServer {
		public Task<ProtocolStatus> Receive(ProtocolMessage data) {
			return Task.FromResult(new ProtocolStatus() { Result = ProtocolResult.Rejected, Role = ProtocolRole.None, SessionId = null });
		}
	}
}
