using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Replay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ReplayClientService : PeriodicService<ResponseLoggerService> {
		private readonly ReplayOptions options;
		private ReplayLMUClient lmuClient;
		private int completed = 0;

		public ReplayClientService(ILogger<ResponseLoggerService> logger, IServiceProvider serviceProvider, IOptions<ReplayOptions> options) : base(logger, serviceProvider) {
			this.options = options.Value ?? new ReplayOptions();
		}

		public override int CalculateDelay() {
			return options.Delay;
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<ReplayLMUClient>();
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
			return lmuClient.Remaining > 0;
		}

		private async Task MakeRequests() {
			await lmuClient.GetSessionInfo();
			await lmuClient.GetMultiplayerTeams();
			await lmuClient.GetStandings();
			await lmuClient.GetChat();
			await lmuClient.GetStrategy();
			await lmuClient.GetStrategyUsage();
			await lmuClient.GetStandingsHistory();
			//await lmuClient.GetTrackMap();
		}

		public override Task End() {
			logger.LogInformation($"Replay finished with {completed} completed runs");
			return Task.CompletedTask;
		}
	}
}
