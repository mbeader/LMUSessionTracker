using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;

namespace LMUSessionTracker.Core.Client {
	public interface ClientHandlerFactory {
		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client);
	}

	public class DefaultClientHandlerFactory : ClientHandlerFactory {
		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client) {
			return new DefaultClientHandler(lmuClient, protocolClient, client);
		}
	}
}
