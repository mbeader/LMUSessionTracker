using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Sockets;
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

		private async Task<T> Post<T, TBody>(string path, TBody body) {
			try {
				string json = JsonConvert.SerializeObject(body);
				HttpResponseMessage res = await httpClient.PostAsync(path, new StringContent(json));
				if(res.StatusCode == System.Net.HttpStatusCode.OK && res.Content != null) {
					string content = await res.Content.ReadAsStringAsync();
					if(!string.IsNullOrEmpty(content)) {
						T result = JsonConvert.DeserializeObject<T>(content);
						return result;
					}
				}
				return default;
			} catch(Exception e) {
				if(e is HttpRequestException hre && hre.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
					logger.LogDebug("Connection refused");
				else if(e is TaskCanceledException tce && tce.InnerException is TimeoutException ite || e is TimeoutException te)
					logger.LogDebug("Connection timeout");
				else
					logger.LogError(e, $"Request failed for endpoint: {path}");
				return default;
			}
		}

		public async Task<ProtocolStatus> Send(ProtocolMessage data) {
			return await Post<ProtocolStatus, ProtocolMessage>("api/Data", data);
		}
	}
}
