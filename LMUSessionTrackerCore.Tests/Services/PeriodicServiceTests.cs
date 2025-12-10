using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Services {
	public class PeriodicServiceTests {
		private readonly Mock<FakeDep> dep;
		private readonly Mock<IServiceScope> scope;
		private readonly FakeService service;

		public class FakeService : PeriodicService<FakeService> {
			private readonly FakeDep dep;

			public FakeService(ILogger<FakeService> logger, IServiceProvider serviceProvider, FakeDep dep) : base(logger, serviceProvider) {
				this.dep = dep;
			}

			public override int CalculateDelay() => dep.CalculateDelay();
			public override Task Start(IServiceScope scope) => dep.Start(scope);
			public override Task<bool> Do() => dep.Do();
			public override Task End() => dep.End();
		}

		public interface FakeDep {
			public int CalculateDelay();
			public Task Start(IServiceScope scope);
			public Task<bool> Do();
			public Task End();
		}

		public PeriodicServiceTests() {
			dep = new Mock<FakeDep>();
			dep.Setup(x => x.Do()).ReturnsAsync(true);
			dep.Setup(x => x.CalculateDelay()).Returns(0);
			scope = new Mock<IServiceScope>();
			Mock<IServiceScopeFactory> scopeFactory = new Mock<IServiceScopeFactory>();
			scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
			Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
			service = new FakeService(Mock.Of<ILogger<FakeService>>(), serviceProvider.Object, dep.Object);
		}

		[Fact(Timeout = 5000)]
		public async Task ExecuteAsync_RunsUntilSuicide() {
			CancellationTokenSource source = new CancellationTokenSource();
			int runs = 0;
			dep.Setup(x => x.Do()).ReturnsAsync(() => runs < 9).Callback(() => runs++);
			await service.StartAsync(source.Token);
			dep.Verify(x => x.Start(It.IsAny<IServiceScope>()), Times.Once);
			dep.Verify(x => x.Do(), Times.Exactly(runs));
			dep.Verify(x => x.CalculateDelay(), Times.Exactly(runs - 1));
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
			dep.Verify(x => x.CalculateDelay(), Times.Exactly(runs));
			dep.Verify(x => x.End(), Times.Once);
		}
	}
}
