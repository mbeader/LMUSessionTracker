using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Replay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ReplayClientService : PeriodicService<ResponseLoggerService> {
		private readonly ReplayOptions options;
		private readonly ReplayCollection collection;
		private readonly JsonSerializerOptions serializerOptions;
		private ReplayLMUClient lmuClient;
		private int completed = 0;
		private DateTime last;

		public ReplayClientService(ILogger<ResponseLoggerService> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime, IOptions<ReplayOptions> options) : base(logger, serviceProvider, dateTime) {
			this.options = options.Value ?? new ReplayOptions();
			collection = new ReplayCollection();
			serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
			serializerOptions.Converters.Add(new NullableDictionaryKeyConverter<int, int>());
			serializerOptions.Converters.Add(new NullableDictionaryKeyConverter<uint, int>());
			serializerOptions.Converters.Add(new NullableDictionaryKeyConverter<long, int>());
			serializerOptions.Converters.Add(new NullableDictionaryKeyConverter<bool, int>());
			last = DateTime.UtcNow;
		}

		public override int GetInterval() {
			return options.Interval;
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<ReplayLMUClient>();
			last = dateTime.UtcNow;
			return Task.CompletedTask;
		}

		public override async Task<bool> Do() {
			lmuClient.OpenContext();
			try {
				await MakeRequests();
				completed++;
			} finally {
				lmuClient.CloseContext();
			}
			DateTime now = dateTime.UtcNow;
			if((now - last).TotalSeconds >= 10.0) {
				last = now;
				logger.LogDebug($"Completed {completed} ({completed/((double)completed+lmuClient.Remaining):P0})");
			}
			return lmuClient.Remaining > 0;
		}

		private async Task MakeRequests() {
			ReplayRun run = new ReplayRun();
			run.SessionInfo = await lmuClient.GetSessionInfo();
			run.MultiplayerTeams = await lmuClient.GetMultiplayerTeams();
			run.Standings = await lmuClient.GetStandings();
			run.Chat = await lmuClient.GetChat();
			run.Strategy = await lmuClient.GetStrategy();
			run.StrategyUsage = await lmuClient.GetStrategyUsage();
			run.StandingsHistory = await lmuClient.GetStandingsHistory();
			run.MultiplayerJoinState = await lmuClient.GetMultiplayerJoinState();
			run.GameState = await lmuClient.GetGameState();
			run.SessionsInfoForEvent = await lmuClient.GetSessionsInfoForEvent();
			//run.TrackMap = await lmuClient.GetTrackMap();
			collection.Add(run);
		}

		public override Task End() {
			logger.LogInformation($"Replay finished with {completed} completed runs");
			string dir = Path.Join("logs", "replay");
			Directory.CreateDirectory(dir);
			File.WriteAllText(Path.Join(dir, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}-replay.json"), JsonSerializer.Serialize(collection.Build(), serializerOptions));
			return Task.CompletedTask;
		}
	}
}
