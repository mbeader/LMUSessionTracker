using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ResponseLoggerService : PeriodicService<ResponseLoggerService> {
		private static readonly int interval = 1000;
		private LMUClient lmuClient;
		private DateTimeProvider dateTime;
		private DateTime last;

		public ResponseLoggerService(ILogger<ResponseLoggerService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider) {
		}

		public override int CalculateDelay() {
			DateTime now = dateTime.UtcNow;
			int toNextInterval = interval - (int)(now - last).TotalMilliseconds;
			return toNextInterval < 0 ? 0 : toNextInterval;
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<LMUClient>();
			dateTime = scope.ServiceProvider.GetRequiredService<DateTimeProvider>();
			last = dateTime.UtcNow;
			return Task.CompletedTask;
		}

		public override async Task<bool> Do() {
			lmuClient.OpenContext();
			try {
				await MakeRequests();
			} finally {
				lmuClient.CloseContext();
			}
			return true;
		}

		private async Task MakeRequests() {
			await lmuClient.GetSessionInfo();
			await lmuClient.GetMultiplayerJoinState();
			await lmuClient.GetMultiplayerTeams();
			await lmuClient.GetStandings();
			await lmuClient.GetChat();
			await lmuClient.GetStrategy();
			await lmuClient.GetStrategyUsage();
			await lmuClient.GetStandingsHistory();
			//await lmuClient.GetProfileInfo();
			await lmuClient.GetGameState();
			await lmuClient.GetSessionsInfoForEvent();
			//await lmuClient.GetTrackMap();
		}

		public override Task End() {
			return Task.CompletedTask;
		}
	}
}
