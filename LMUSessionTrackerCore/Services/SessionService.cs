using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class SessionService : PeriodicService<SessionService> {
		private SessionManager manager;
		private LMUClient client;

		public SessionService(ILogger<SessionService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider) {
		}

		public override int CalculateDelay() {
			return 1000;
		}

		public override Task Start(IServiceScope scope) {
			manager = scope.ServiceProvider.GetRequiredService<SessionManager>();
			client = scope.ServiceProvider.GetRequiredService<LMUClient>();
			return Task.CompletedTask;
		}

		public override async Task Do() {
			SessionInfo info = await client.GetSessionInfo();
			manager.UpdateSession(info);
			if(info != null) {
				List<Standing> standings = await client.GetStandings();
				manager.UpdateStandings(standings);
			}
			manager.PeriodicPersist();
		}

		public override Task End() {
			manager.Persist();
			return Task.CompletedTask;
		}
	}
}
