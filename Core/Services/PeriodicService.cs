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

		public PeriodicService(ILogger<T> logger, IServiceProvider serviceProvider) {
			this.logger = logger;
			this.serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			logger.LogInformation("Starting service");
			using(IServiceScope scope = serviceProvider.CreateScope()) {
				await Start(scope);
				while(!stoppingToken.IsCancellationRequested) {
					if(!await Do())
						break;
					await Task.Delay(CalculateDelay(), stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
				}
				await End();
				logger.LogInformation("Stopping service");
			}
		}

		public abstract int CalculateDelay();

		public abstract Task Start(IServiceScope scope);

		public abstract Task<bool> Do();

		public abstract Task End();
	}
}
