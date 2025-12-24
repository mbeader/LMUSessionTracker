using LMUSessionTracker.Core.Replay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ReplayClientService : PeriodicService<ResponseLoggerService> {
		private readonly ReplayOptions options;
		private readonly ReplayCollection collection;
		private ReplayLMUClient lmuClient;
		private DateTimeProvider dateTime;
		private int completed = 0;
		private DateTime last;

		public ReplayClientService(ILogger<ResponseLoggerService> logger, IServiceProvider serviceProvider, IOptions<ReplayOptions> options) : base(logger, serviceProvider) {
			this.options = options.Value ?? new ReplayOptions();
			collection = new ReplayCollection();
			last = DateTime.UtcNow;
		}

		public override int CalculateDelay() {
			DateTime now = dateTime.UtcNow;
			int toNextInterval = options.Interval - (int)(now - last).TotalMilliseconds;
			return toNextInterval < 0 ? 0 : toNextInterval;
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<ReplayLMUClient>();
			dateTime = scope.ServiceProvider.GetRequiredService<DateTimeProvider>();
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
			DateTime now = DateTime.UtcNow;
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
			File.WriteAllText(Path.Join(dir, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}-replay.json"), JsonConvert.SerializeObject(collection.Build(), Formatting.Indented));
			return Task.CompletedTask;
		}
	}
}
