using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Replay {
	public class ReplayLMUClient : LMUClient {
		private static readonly Regex filenamePattern = new Regex(@"\d{17}-raw.json", RegexOptions.Compiled);
		private readonly ILogger<ReplayLMUClient> logger;
		private readonly ReplayOptions options;
		private readonly SchemaValidator schemaValidator;
		private readonly JsonSerializerSettings serializerSettings;
		private readonly string path;
		private readonly Queue<string> runQueue;
		private Dictionary<string, string> context;

		public string ContextId { get; private set; }
		public int Remaining => runQueue.Count;

		public ReplayLMUClient(ILogger<ReplayLMUClient> logger, IOptions<ReplayOptions> options, SchemaValidator schemaValidator = null) {
			this.logger = logger;
			this.options = options.Value ?? new ReplayOptions();
			this.schemaValidator = schemaValidator;
			if(string.IsNullOrEmpty(this.options.Directory))
				throw new Exception("Replay directory not specified");
			path = Path.GetFullPath(this.options.Directory);
			if(!Directory.Exists(path))
				throw new Exception($"Replay directory does not exist: {path}");
			runQueue = LoadDirectory(path);
			serializerSettings = new JsonSerializerSettings();
			serializerSettings.Converters.Add(new TeamStrategyConverter());
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
			if(context.TryGetValue(path, out string content))
				return content;
			return null;
		}

		private Task<T> Get<T>(string path) {
			try {
				string body = GetContent(path);
				if(!string.IsNullOrEmpty(body)) {
					T result = JsonConvert.DeserializeObject<T>(body, serializerSettings);
					if(result != null && options.ValidateResponses && schemaValidator != null) {
						string runId = Path.GetFileName(ContextId)[..17];
						schemaValidator.Validate(body, typeof(T), runId);
					}
					return Task.FromResult(result);
				}
				return Task.FromResult<T>(default);
			} catch(Exception e) {
				logger.LogError(e, $"Read failed for endpoint: {path}");
				return Task.FromResult<T>(default);
			}
		}

		public void OpenContext() {
			if(ContextId == null || context == null) {
				ResetContext();
			}
			if(runQueue.TryDequeue(out string filename)) {
				JsonSerializer serializer = new JsonSerializer();
				using(StreamReader file = File.OpenText(filename))
				using(JsonTextReader reader = new JsonTextReader(file)) {
					context = serializer.Deserialize<Dictionary<string, string>>(reader);
					ContextId = filename;
				}
			} else
				logger.LogWarning("Queue is empty");
		}

		public void CloseContext() {
			ResetContext();
		}

		private void ResetContext() {
			context = null;
			ContextId = null;
		}

		public Task<List<Chat>> GetChat() {
			return Get<List<Chat>>("/rest/chat");
		}

		public Task<MultiplayerTeams> GetMultiplayerTeams() {
			return Get<MultiplayerTeams>("/rest/multiplayer/teams");
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
