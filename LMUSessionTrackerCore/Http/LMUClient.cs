using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public class LMUClient {
		private readonly HttpClient httpClient;
		private readonly ILogger<LMUClient> logger;
		private readonly SchemaValidator schemaValidator;
		private readonly LMUClientOptions options;

		public LMUClient(ILogger<LMUClient> logger, SchemaValidator schemaValidator = null, IOptions<LMUClientOptions> options = null) {
			this.logger = logger;
			this.schemaValidator = schemaValidator;
			this.options = options?.Value ?? new LMUClientOptions();
			httpClient = new HttpClient() {
				BaseAddress = new Uri(this.options.BaseUri),
				Timeout = new TimeSpan(this.options.TimeoutSeconds * TimeSpan.TicksPerSecond)
			};
		}

		private async Task<T> Get<T>(string path) {
			try {
				HttpResponseMessage res = await httpClient.GetAsync(path);
				if(res.StatusCode == System.Net.HttpStatusCode.OK && res.Content != null) {
					string body = await res.Content.ReadAsStringAsync();
					if(!string.IsNullOrEmpty(body)) {
						T result = JsonConvert.DeserializeObject<T>(body);
						if(options.ValidateResponses && schemaValidator != null)
							schemaValidator.Validate(body, typeof(T));
						if(options.LogResponses && result != null)
							LogResponse(path, result);
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

		private void LogResponse<T>(string path, T response) {
			try {
				if(!Directory.Exists(options.LogDirectory))
					Directory.CreateDirectory(options.LogDirectory);
				string json = JsonConvert.SerializeObject(response);
				File.WriteAllText(Path.Join(options.LogDirectory, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{path.Replace("/", "_")}.json"), json);
			} catch(Exception e) {
				logger.LogWarning(e, "Failed to write request debug file");
			}
		}

		public Task<SessionInfo> GetSessionInfo() {
			return Get<SessionInfo>("/rest/watch/sessionInfo");
		}

		public Task<List<Standing>> GetStandings() {
			return Get<List<Standing>>("/rest/watch/standings");
		}
	}
}