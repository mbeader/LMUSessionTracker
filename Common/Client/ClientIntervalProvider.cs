using LMUSessionTracker.Common.Protocol;
using LMUSessionTracker.Common.Services;

namespace LMUSessionTracker.Common.Client {
	public interface ClientIntervalProvider : IntervalProvider {
		public void SetInterval(ClientHandler handler, ProtocolMessage message);
	}

	public class DefaultClientIntervalProvider : DefaultIntervalProvider, ClientIntervalProvider {
		public DefaultClientIntervalProvider(ClientInfo client) : base(client) { }

		public void SetInterval(ClientHandler handler, ProtocolMessage message) { }
	}
}
