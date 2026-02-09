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
			AssertPoint2D(new Point2D() { X = -60.710132598876953, Y = 3.9064428806304932 }, ac.S1[0]);
			AssertPoint2D(new Point2D() { X = -490.14181518554688, Y = 426.76171875 }, ac.S2[0]);
			AssertPoint2D(new Point2D() { X = 465.37982177734375, Y = 360.81195068359375 }, ac.S3[0]);
			Assert.Equal(423, ac.S1.Count);
			Assert.Equal(301, ac.S2.Count);
			Assert.Equal(452, ac.S3.Count);
			Assert.Equal(1176, ac.S1.Count + ac.S2.Count + ac.S3.Count);
		}
	}
}
