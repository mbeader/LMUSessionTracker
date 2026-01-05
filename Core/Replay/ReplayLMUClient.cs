using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Replay {
	public class ReplayLMUClient : LMUClient {
		private static readonly Regex filenamePattern = new Regex(@"\d{17}-raw.json", RegexOptions.Compiled);
		private readonly ILogger<ReplayLMUClient> logger;
		private readonly ReplayOptions options;
		private readonly SchemaValidator schemaValidator;
		private readonly ContinueProviderSource continueProviderSource;
		private readonly JsonSerializerOptions serializerOptions;
		private readonly string path;
		private readonly Queue<string> runQueue;
		private Dictionary<string, string> context;

		public string ContextId { get; private set; }
		public int Remaining => runQueue.Count;

		public ReplayLMUClient(ILogger<ReplayLMUClient> logger, IOptions<ReplayOptions> options, SchemaValidator schemaValidator = null, ContinueProviderSource continueProviderSource = null) {
			this.logger = logger;
			this.options = options.Value ?? new ReplayOptions();
			this.schemaValidator = schemaValidator;
			this.continueProviderSource = continueProviderSource;
			if(string.IsNullOrEmpty(this.options.Directory))
				throw new Exception("Replay directory not specified");
			path = Path.GetFullPath(this.options.Directory);
			if(!Directory.Exists(path))
				throw new Exception($"Replay directory does not exist: {path}");
			runQueue = LoadDirectory(path);
			serializerOptions = new JsonSerializerOptions();
			serializerOptions.Converters.Add(new TeamStrategyConverter());
			logger.LogInformation($"Found {runQueue.Count} runs to replay");
		}

		private Queue<string> LoadDirectory(string path) {
			string[] files = Directory.GetFiles(path);
			Queue<string> queue = new Queue<string>(files.Length);
			Array.Sort(files, StringComparer.OrdinalIgnoreCase);
			foreach(string file in files) {
				if(filenamePattern.IsMatch(file))
					queue.Enqueue(file);
			}
			return queue;
		}

		private string GetContent(string path) {
			if(context != null && context.TryGetValue(path, out string content))
				return content;
			return null;
		}

		private Task<T> Get<T>(string path) {
			try {
				string body = GetContent(path);
				if(!string.IsNullOrEmpty(body)) {
					T result = JsonSerializer.Deserialize<T>(body, serializerOptions);
					if(result != null && options.ValidateResponses && schemaValidator != null) {
						string runId = Path.GetFileName(ContextId)[..17];
						schemaValidator.Validate(body, typeof(T), runId);
					}
					return Task.FromResult(result);
				}
				return Task.FromResult<T>(default);
			} catch(Exception e) {
				logger.LogError(e, $"Read failed for endpoint: {path}, context: {ContextId}");
				return Task.FromResult<T>(default);
			}
		}

		public void OpenContext() {
			if(ContextId == null || context == null) {
				ResetContext();
			}
			if(runQueue.TryDequeue(out string filename)) {
				using(FileStream stream = File.OpenRead(filename)) {
					context = JsonSerializer.Deserialize<Dictionary<string, string>>(stream);
					ContextId = filename;
				}
			} else
				logger.LogWarning("Queue is empty");
		}

		public void CloseContext() {
			ResetContext();
			if(runQueue.Count == 0 && continueProviderSource != null) {
				continueProviderSource.Continue = false;
				logger.LogInformation("Replay finished");
			}
		}

		private void ResetContext() {
			context = null;
			ContextId = null;
		}

		public Task<List<Chat>> GetChat() {
			return Get<List<Chat>>("/rest/chat");
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
