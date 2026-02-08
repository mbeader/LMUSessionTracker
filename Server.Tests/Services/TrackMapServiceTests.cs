using LMUSessionTracker.Server.Services;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Tests.Services {
	public class TrackMapServiceTests {
		private readonly TrackMapService service;

		public TrackMapServiceTests(LoggingFixture loggingFixture) {
			service = new TrackMapService(loggingFixture.LoggerFactory.CreateLogger<TrackMapService>());
		}

		[Fact]
		public void SetMetadata_Sebring_SplitsIntoSectors() {
			string track = "Sebring International Raceway";
			List<Point2D> ex = service.GetTrack(track).Points;
			service.SetMetadata(track, 10, 433, 734);
			TrackMap ac = service.GetTrack(track);
			Assert.Equal(ex[10], ac.S1[0]);
			Assert.Equal(ex[433], ac.S2[0]);
			Assert.Equal(ex[734], ac.S3[0]);
			Assert.Equal(ex.Count, ac.S1.Count + ac.S2.Count + ac.S3.Count);
		}
	}
}
