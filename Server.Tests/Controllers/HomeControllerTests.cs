using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Controllers;
using LMUSessionTracker.Server.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LMUSessionTracker.Server.Tests.Controllers {
	public class HomeControllerTests {
		private readonly Mock<SessionRepository> sessionRepo;
		private readonly Mock<SessionObserver> sessionObserver;
		private readonly HomeController controller;

		public HomeControllerTests(LoggingFixture loggingFixture) {
			sessionRepo = new Mock<SessionRepository>();
			sessionObserver = new Mock<SessionObserver>();
			controller = new HomeController(loggingFixture.LoggerFactory.CreateLogger<HomeController>(), sessionRepo.Object, sessionObserver.Object);
		}

		[Fact]
		public void Construct() {
			Assert.NotNull(controller);
		}
	}
}
