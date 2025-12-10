using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ResponseLoggerService : PeriodicService<ResponseLoggerService> {
		private LMUClient lmuClient;

		public ResponseLoggerService(ILogger<ResponseLoggerService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider) {
		}

		public override int CalculateDelay() {
			return 1000;
		}

		public override Task Start(IServiceScope scope) {
			lmuClient = scope.ServiceProvider.GetRequiredService<LMUClient>();
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
			await lmuClient.GetMultiplayerTeams();
			await lmuClient.GetStandings();
			await lmuClient.GetChat();
			await lmuClient.GetStrategy();
			await lmuClient.GetStrategyUsage();
			await lmuClient.GetStandingsHistory();
			//await lmuClient.GetTrackMap();
		}

		public override Task End() {
			return Task.CompletedTask;
		}
	}
}
