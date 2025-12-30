using LMUSessionTracker.Core.Tracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class SessionCleanupService : PeriodicService<SessionCleanupService> {
		private SessionArbiter arbiter;

		public SessionCleanupService(ILogger<SessionCleanupService> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime) : base(logger, serviceProvider, dateTime) {
		}

		public override int GetInterval() {
			return 60000;
		}

		public override Task Start(IServiceScope scope) {
			arbiter = scope.ServiceProvider.GetRequiredService<SessionArbiter>();
			return Task.CompletedTask;
		}

		public override async Task<bool> Do() {
			await arbiter.Prune(dateTime.UtcNow);
			return true;
		}

		public override Task End() {
			return Task.CompletedTask;
		}
	}
}
