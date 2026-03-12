using LMUSessionTracker.Core.Tracking;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class SessionLoaderService : BackgroundService {
		private readonly SessionArbiter arbiter;

		public SessionLoaderService(SessionArbiter arbiter) {
			this.arbiter = arbiter;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			await arbiter.Load();
		}
	}
}
