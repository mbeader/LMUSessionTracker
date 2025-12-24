using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public abstract class PeriodicService<T> : BackgroundService {
		protected readonly ILogger<T> logger;
		protected readonly IServiceProvider serviceProvider;
		protected readonly DateTimeProvider dateTime;
		private DateTime next;

		public DateTime Next => next;

		public PeriodicService(ILogger<T> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime) {
			this.logger = logger;
			this.serviceProvider = serviceProvider;
			this.dateTime = dateTime;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			logger.LogInformation("Starting service");
			DateTime now = dateTime.UtcNow;
			next = now.AddMilliseconds(GetInterval());
			using(IServiceScope scope = serviceProvider.CreateScope()) {
				await Start(scope);
				while(!stoppingToken.IsCancellationRequested) {
					if(!await Do())
						break;
					now = dateTime.UtcNow;
					int delay = CalculateDelay(now, next);
					await Delay(delay, stoppingToken);
					next = (delay == 0 ? now : next).AddMilliseconds(GetInterval());
				}
				await End();
				logger.LogInformation("Stopping service");
			}
		}

		protected virtual async Task Delay(int delay, CancellationToken stoppingToken) => await Task.Delay(delay, stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

		private int CalculateDelay(DateTime now, DateTime next) {
			TimeSpan diff = next - now;
			if(diff < TimeSpan.Zero)
				return 0;
			else
				return (int)diff.TotalMilliseconds;
		}

		public abstract int GetInterval();

		public abstract Task Start(IServiceScope scope);

		public abstract Task<bool> Do();

		public abstract Task End();
	}
}
