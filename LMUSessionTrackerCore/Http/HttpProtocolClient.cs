using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public class HttpProtocolClient : ProtocolClient {
		private readonly HttpClient httpClient;
		private readonly ILogger<HttpProtocolClient> logger;
		private readonly ProtocolClientOptions options;

		public HttpProtocolClient(ILogger<HttpProtocolClient> logger, IOptions<ProtocolClientOptions> options = null) {
			this.logger = logger;
			this.options = options?.Value ?? new ProtocolClientOptions();
			httpClient = new HttpClient() {
				BaseAddress = new Uri(this.options.BaseUri),
				Timeout = new TimeSpan(this.options.TimeoutSeconds * TimeSpan.TicksPerSecond)
			};
		}

		public Task<ProtocolStatus> Send(ProtocolMessage data) {
			return Task.FromResult(new ProtocolStatus() { Role = ProtocolRole.None, Result = ProtocolResult.Rejected });
		}
	}
}
