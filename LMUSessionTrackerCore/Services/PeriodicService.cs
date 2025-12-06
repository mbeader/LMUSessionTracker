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
					await Do();
					await Task.Delay(CalculateDelay(), stoppingToken);
				}
				await End();
				logger.LogInformation("Stopping service");
			}
		}

		public abstract int CalculateDelay();

		public abstract Task Start(IServiceScope scope);

		public abstract Task Do();

		public abstract Task End();
	}
}
