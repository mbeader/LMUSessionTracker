using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Http {
	public class HttpLMUClient : LMUClient {
		private readonly HttpClient httpClient;
		private readonly ILogger<HttpLMUClient> logger;
		private readonly SchemaValidator schemaValidator;
		private readonly LMUClientOptions options;
		private readonly JsonSerializerOptions serializerOptions;
		private readonly string logPath;
		private string contextId = null;
		//private readonly ConcurrentDictionary<string, object> objContext = new ConcurrentDictionary<string, object>();
		private readonly ConcurrentDictionary<string, string> rawContext = new ConcurrentDictionary<string, string>();

		public HttpLMUClient(ILogger<HttpLMUClient> logger, SchemaValidator schemaValidator = null, IOptions<LMUClientOptions> options = null) {
			this.logger = logger;
			this.schemaValidator = schemaValidator;
			this.options = options?.Value ?? new LMUClientOptions();
			httpClient = new HttpClient() {
				BaseAddress = new Uri(this.options.BaseUri),
				Timeout = new TimeSpan(this.options.TimeoutSeconds * TimeSpan.TicksPerSecond)
			};
			serializerOptions = new JsonSerializerOptions();
			serializerOptions.Converters.Add(new TeamStrategyConverter());
			if(this.options.LogResponses && string.IsNullOrEmpty(this.options.LogDirectory))
				throw new Exception("Directory for response logging must be specified");
			logPath = Path.GetFullPath(this.options.LogDirectory);
		}

		private async Task<T> Get<T>(string path) {
			try {
				HttpResponseMessage res = await httpClient.GetAsync(path);
				if(res.StatusCode == System.Net.HttpStatusCode.OK && res.Content != null) {
					string body = await res.Content.ReadAsStringAsync();
					if(options.LogResponses)
						rawContext.TryAdd(path, body);
					if(!string.IsNullOrEmpty(body)) {
						T result = JsonSerializer.Deserialize<T>(body, serializerOptions);
						if(result != null && options.ValidateResponses && schemaValidator != null)
							schemaValidator.Validate(body, typeof(T));
						//if(options.LogResponses)
						//	objContext.Add(path, result);
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
				if(!Directory.Exists(logPath))
					Directory.CreateDirectory(logPath);
				string json = JsonSerializer.Serialize(response, serializerOptions);
				File.WriteAllText(Path.Join(logPath, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{path.Replace("/", "_")}.json"), json);
			} catch(Exception e) {
				logger.LogWarning(e, "Failed to write request debug file");
			}
		}

		private void LogContext() {
			if(contextId == null || !options.LogResponses)
				return;
			try {
				if(!Directory.Exists(logPath))
					Directory.CreateDirectory(logPath);
				//File.WriteAllText(Path.Join(logPath, $"{contextId}-obj.json"), JsonSerializer.Serialize(objContext, new JsonSerializerOptions(serializerOptions) { WriteIndented = true }));
				File.WriteAllText(Path.Join(logPath, $"{contextId}-raw.json"), JsonSerializer.Serialize(rawContext, serializerOptions));
			} catch(Exception e) {
				logger.LogWarning(e, "Failed to write request debug file");
			}
		}

		public void OpenContext() {
			if(contextId != null) {
				logger.LogWarning("A leftover context was not logged");
				ResetContext();
			}
			contextId = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
		}

		public void CloseContext() {
			if(contextId == null)
				return;
			LogContext();
			ResetContext();
		}

		private void ResetContext() {
			contextId = null;
			//objContext.Clear();
			rawContext.Clear();
		}

		public Task<List<Chat>> GetChat() {
			return Get<List<Chat>>("/rest/chat/");
		}

		public Task<string> GetMultiplayerJoinState() {
			return Get<string>("/rest/multiplayer/join/state");
		}

		public Task<MultiplayerTeams> GetMultiplayerTeams() {
			return Get<MultiplayerTeams>("/rest/multiplayer/teams");
		}

		public Task<ProfileInfo> GetProfileInfo() {
			return Get<ProfileInfo>("/rest/profile/profileInfo/getProfileInfo");
		}

		public Task<GameState> GetGameState() {
			return Get<GameState>("/rest/sessions/GetGameState");
		}

		public Task<SessionsInfoForEvent> GetSessionsInfoForEvent() {
			return Get<SessionsInfoForEvent>("/rest/sessions/GetSessionsInfoForEvent");
		}

		public Task<List<TeamStrategy>> GetStrategy() {
			return Get<List<TeamStrategy>>("/rest/strategy/overall");
		}

		public Task<StrategyUsage> GetStrategyUsage() {
			return Get<StrategyUsage>("/rest/strategy/usage");
		}

		public Task<SessionInfo> GetSessionInfo() {
			return Get<SessionInfo>("/rest/watch/sessionInfo");
		}

		public Task<List<Standing>> GetStandings() {
			return Get<List<Standing>>("/rest/watch/standings");
		}

		public Task<StandingsHistory> GetStandingsHistory() {
			return Get<StandingsHistory>("/rest/watch/standings/history");
		}

		public Task<List<TrackMapPoint>> GetTrackMap() {
			return Get<List<TrackMapPoint>>("/rest/watch/trackmap");
		}
	}
}