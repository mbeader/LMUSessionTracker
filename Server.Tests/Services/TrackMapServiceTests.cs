using LMUSessionTracker.Server.Services;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.Extensions.Logging;

namespace LMUSessionTracker.Server.Tests.Services {
	public class TrackMapServiceTests {
		private readonly TrackMapService service;

		public TrackMapServiceTests(LoggingFixture loggingFixture) {
			service = new TrackMapService(loggingFixture.LoggerFactory.CreateLogger<TrackMapService>());
		}

		private void AssertPoint2D(Point2D ex, Point2D ac) {
			Assert.Equal(ex.X, ac.X);
			Assert.Equal(ex.Y, ac.Y);
		}

		[Fact]
		public void GetTrack_Sebring_ReturnsSplitIntoSectors() {
			TrackMap ac = service.GetTrack("Sebring International Raceway");
			AssertPoint2D(new Point2D() { X = -100.30023193359375, Y = 4.9445900917053223 }, ac.S1[0]);
			AssertPoint2D(new Point2D() { X = -496.26275634765625, Y = 419.06680297851563 }, ac.S2[0]);
			AssertPoint2D(new Point2D() { X = 465.1531982421875, Y = 390.49862670898438 }, ac.S3[0]);
			Assert.Equal(429, ac.S1.Count);
			Assert.Equal(297, ac.S2.Count);
			Assert.Equal(450, ac.S3.Count);
			Assert.Equal(1176, ac.S1.Count + ac.S2.Count + ac.S3.Count);
		}
	}
}
