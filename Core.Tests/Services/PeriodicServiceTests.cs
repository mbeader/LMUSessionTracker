using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Services {
	public class PeriodicServiceTests {
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private readonly Mock<FakeDep> dep;
		private readonly Mock<DateTimeProvider> dateTime;
		private readonly Mock<IServiceScope> scope;
		private readonly FakeService service;

		public class FakeService : PeriodicService<FakeService> {
			private readonly FakeDep dep;

			public List<int> Delays { get; } = new List<int>();

			public FakeService(ILogger<FakeService> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime, FakeDep dep) : base(logger, serviceProvider, dateTime) {
				this.dep = dep;
			}

			protected override Task Delay(int delay, CancellationToken stoppingToken) {
				Delays.Add(delay);
				return Task.CompletedTask;
			}

			public override int GetInterval() => dep.GetInterval();
			public override Task Start(IServiceScope scope) => dep.Start(scope);
			public override Task<bool> Do() => dep.Do();
			public override Task End() => dep.End();
		}

		public interface FakeDep {
			public int GetInterval();
			public Task Start(IServiceScope scope);
			public Task<bool> Do();
			public Task End();
		}

		public PeriodicServiceTests(LoggingFixture loggingFixture) {
			dep = new Mock<FakeDep>();
			dep.Setup(x => x.Do()).ReturnsAsync(true);
			dep.Setup(x => x.GetInterval()).Returns(0);
			dateTime = new Mock<DateTimeProvider>();
			dateTime.Setup(x => x.UtcNow).Returns(baseTimestamp);
			scope = new Mock<IServiceScope>();
			Mock<IServiceScopeFactory> scopeFactory = new Mock<IServiceScopeFactory>();
			scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
			Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
			service = new FakeService(loggingFixture.LoggerFactory.CreateLogger<FakeService>(), serviceProvider.Object, dateTime.Object, dep.Object);
		}

		[Fact(Timeout = 5000)]
		public async Task ExecuteAsync_RunsUntilSuicide() {
			CancellationTokenSource source = new CancellationTokenSource();
			int runs = 0;
			dep.Setup(x => x.Do()).ReturnsAsync(() => runs < 9).Callback(() => runs++);
			await service.StartAsync(source.Token);
			dep.Verify(x => x.Start(It.IsAny<IServiceScope>()), Times.Once);
			dep.Verify(x => x.Do(), Times.Exactly(runs));
			dep.Verify(x => x.GetInterval(), Times.Exactly(runs));
			dep.Verify(x => x.End(), Times.Once);
		}

		[Fact(Timeout = 5000)]
		public async Task ExecuteAsync_RunsUntilCanceled() {
			CancellationTokenSource source = new CancellationTokenSource();
			int runs = 0;
			dep.Setup(x => x.Do()).ReturnsAsync(true).Callback(() => { runs++; if(runs >= 10) source.Cancel(); });
			await service.StartAsync(source.Token);
			dep.Verify(x => x.Start(It.IsAny<IServiceScope>()), Times.Once);
			dep.Verify(x => x.Do(), Times.Exactly(runs));
			dep.Verify(x => x.GetInterval(), Times.Exactly(runs + 1));
			dep.Verify(x => x.End(), Times.Once);
		}

		[Theory]
		[InlineData(1000, 0, 2000, 1000)]
		[InlineData(1000, 200, 2000, 800)]
		[InlineData(1000, 1000, 2000, 0)]
		[InlineData(1000, 2000, 3000, 0)]
		public async Task ExecuteAsync_GivenIntervalAndElapsed_RunsUntilSuicide(int interval, int elapsed, int expectedNext, int expectedDelay) {
			CancellationTokenSource source = new CancellationTokenSource();
			dateTime.SetupSequence(x => x.UtcNow).Returns(() => baseTimestamp).Returns(() => baseTimestamp.AddMilliseconds(elapsed));
			dep.Setup(x => x.GetInterval()).Returns(interval);
			dep.SetupSequence(x => x.Do()).ReturnsAsync(true).ReturnsAsync(false);
			await service.StartAsync(source.Token);
			Assert.Equal(baseTimestamp.AddMilliseconds(expectedNext), service.Next);
			Assert.Equal(new List<int>() { expectedDelay }, service.Delays);
			dep.Verify(x => x.Start(It.IsAny<IServiceScope>()), Times.Once);
			dep.Verify(x => x.Do(), Times.Exactly(2));
			dep.Verify(x => x.GetInterval(), Times.Exactly(2));
			dep.Verify(x => x.End(), Times.Once);
		}
	}
}
