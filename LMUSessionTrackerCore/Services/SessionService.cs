using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class SessionService : BackgroundService {
		private readonly ILogger<SessionService> logger;
		private readonly IServiceProvider serviceProvider;

		public SessionService(ILogger<SessionService> logger, IServiceProvider serviceProvider) {
			this.logger = logger;
			this.serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			logger.LogInformation("Starting service");
			using(IServiceScope scope = serviceProvider.CreateScope()) {
				SessionManager manager = scope.ServiceProvider.GetRequiredService<SessionManager>();
				LMUClient client = scope.ServiceProvider.GetRequiredService<LMUClient>();
				while(!stoppingToken.IsCancellationRequested) {
					SessionInfo info = await client.GetSessionInfo();
					manager.UpdateSession(info);
					if(info != null) {
						List<Standing> standings = await client.GetStandings();
						manager.UpdateStandings(standings);
					}
					manager.PeriodicPersist();
					await Task.Delay(1000, stoppingToken);
				}
				manager.Persist();
				logger.LogInformation("Stopping service");
			}
		}
	}
}
