using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.LMU {
	public class LMUClient {
		private static readonly bool debug = false;
		private static readonly string debugdir = "debug";
		private static readonly string baseuri = "http://localhost:6397/";
		private static readonly TimeSpan timeout = new TimeSpan(2 * TimeSpan.TicksPerSecond);
		private readonly HttpClient httpClient;
		private readonly ILogger<LMUClient> logger;

		public LMUClient(ILogger<LMUClient> logger) {
			httpClient = new HttpClient() { BaseAddress = new Uri(baseuri), Timeout = timeout };
			this.logger = logger;
		}

		private async Task<T> Get<T>(string path) {
			try {
				HttpResponseMessage res = await httpClient.GetAsync(path);
				if(res.StatusCode == System.Net.HttpStatusCode.OK && res.Content != null) {
					string body = await res.Content.ReadAsStringAsync();
					if(!string.IsNullOrEmpty(body)) {
						T result = JsonConvert.DeserializeObject<T>(body);
						if(debug && result != null)
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
				if(!Directory.Exists(debugdir))
					Directory.CreateDirectory(debugdir);
				string json = JsonConvert.SerializeObject(response);
				File.WriteAllText(Path.Join(debugdir, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{path.Replace("/", "_")}.json"), json);
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