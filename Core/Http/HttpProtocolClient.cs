using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSec.Cryptography;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public class HttpProtocolClient : ProtocolClient {
		private static readonly SignatureAlgorithm algorithm = SignatureAlgorithm.Ed25519;
		private readonly HttpClient httpClient;
		private readonly ILogger<HttpProtocolClient> logger;
		private readonly ProtocolSigningKey signingKey;
		private readonly ProtocolClientOptions options;

		public HttpProtocolClient(ILogger<HttpProtocolClient> logger, ProtocolSigningKey signingKey, IOptions<ProtocolClientOptions> options = null) {
			this.logger = logger;
			this.signingKey = signingKey;
			this.options = options?.Value ?? new ProtocolClientOptions();
			httpClient = new HttpClient() {
				BaseAddress = new Uri(this.options.BaseUri),
				Timeout = new TimeSpan(this.options.TimeoutSeconds * TimeSpan.TicksPerSecond),
			};
		}

		private async Task<T> Post<T, TBody>(string path, TBody body, bool allowAuth = true) {
			try {
				string json = JsonConvert.SerializeObject(body);
				HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, path);
				req.Content = new StringContent(json, Encoding.UTF8, "application/json");
				string signatureHeader = Convert.ToBase64String(algorithm.Sign(signingKey.Key, Encoding.UTF8.GetBytes(json)));
				req.Headers.Add("X-Signature", signatureHeader);
				HttpResponseMessage res = await httpClient.SendAsync(req);
				if(res.StatusCode == HttpStatusCode.Unauthorized && allowAuth) {
					// authenticate and reattempt
					bool authed = await Authenticate();
					if(!authed) {
						logger.LogWarning("Authentication failed");
						return default;
					}
					HttpRequestMessage req2 = new HttpRequestMessage(HttpMethod.Post, path);
					req2.Content = req.Content;
					req2.Headers.Add("X-Signature", signatureHeader);
					res = await httpClient.SendAsync(req2);
				}
				if(res.StatusCode == HttpStatusCode.OK && res.Content != null) {
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

		private async Task<bool> Authenticate() {
			return await Post<bool, string>("api/Data/Authenticate", Convert.ToBase64String(signingKey.Key.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), false);
		}

		public async Task<ProtocolStatus> Send(ProtocolMessage data) {
			return await Post<ProtocolStatus, ProtocolMessage>("api/Data", data);
		}
	}
}
