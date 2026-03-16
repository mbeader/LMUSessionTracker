using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;

namespace LMUSessionTracker.Common.Client {
	public interface ClientHandlerFactory {
		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client, ClientIntervalProvider interval);
	}

	public class DefaultClientHandlerFactory : ClientHandlerFactory {
		private readonly ILoggerFactory loggerFactory;

		public DefaultClientHandlerFactory(ILoggerFactory loggerFactory) {
			this.loggerFactory = loggerFactory;
		}

		public ClientHandler Create(LMUClient lmuClient, ProtocolClient protocolClient, ClientInfo client, ClientIntervalProvider interval) {
			return new DefaultClientHandler(loggerFactory.CreateLogger<DefaultClientHandler>(), lmuClient, protocolClient, client, interval);
		}
	}
}
