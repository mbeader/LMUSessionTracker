using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace LMUSessionTracker.Core.Client {
	public interface ClientHandlerFactory {
		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client);
	}

	public class DefaultClientHandlerFactory : ClientHandlerFactory {
		private readonly ILoggerFactory loggerFactory;

		public DefaultClientHandlerFactory(ILoggerFactory loggerFactory) {
			this.loggerFactory = loggerFactory;
		}

		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client) {
			return new DefaultClientHandler(loggerFactory.CreateLogger<DefaultClientHandler>(), lmuClient, protocolClient, client);
		}
	}
}
