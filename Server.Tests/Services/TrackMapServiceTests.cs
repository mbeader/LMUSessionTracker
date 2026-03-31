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

		[Fact]
		public void GetTrack_SilverstoneELMS_ReturnsSplitIntoSectors() {
			TrackMap ac = service.GetTrack("Silverstone Grand Prix Circuit - ELMS");
			AssertPoint2D(new Point2D() { X = 7.3047223091125488, Y = 846.07476806640625 }, ac.S1[0]);
			AssertPoint2D(new Point2D() { X = 379.394287109375, Y = -214.94035339355469 }, ac.S2[0]);
			AssertPoint2D(new Point2D() { X = -338.74752807617188, Y = -213.18675231933594 }, ac.S3[0]);
			Assert.Equal(315, ac.S1.Count);
			Assert.Equal(342, ac.S2.Count);
			Assert.Equal(517, ac.S3.Count);
			Assert.Equal(1174, ac.S1.Count + ac.S2.Count + ac.S3.Count);
		}

		[Fact]
		public void GetTrack_SilverstoneWEC_ReturnsSplitIntoSectors() {
			TrackMap ac = service.GetTrack("Silverstone Grand Prix Circuit - WEC");
			AssertPoint2D(new Point2D() { X = -335.74002075195313, Y = -209.18667602539063 }, ac.S1[0]);
			AssertPoint2D(new Point2D() { X = -41.362625122070313, Y = 620.6954345703125 }, ac.S2[0]);
			AssertPoint2D(new Point2D() { X = 381.83755493164063, Y = -210.57199096679688 }, ac.S3[0]);
			Assert.Equal(344, ac.S1.Count);
			Assert.Equal(486, ac.S2.Count);
			Assert.Equal(344, ac.S3.Count);
			Assert.Equal(1174, ac.S1.Count + ac.S2.Count + ac.S3.Count);
		}
	}
}
