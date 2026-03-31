using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Server.Services;
using LMUSessionTracker.Server.Tracking;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Tests.Tracking {
	public class TrackMapBuilderTests {
		private readonly Mock<TrackMapService> trackMapService;
		private readonly TrackMapBuilder builder;
		private bool needsMetadata = true;

		public TrackMapBuilderTests(LoggingFixture loggingFixture) {
			trackMapService = new Mock<TrackMapService>(Mock.Of<ILogger<TrackMapService>>());
			trackMapService.Setup(x => x.GetTrack("track")).Returns(UnitCircle());
			trackMapService.Setup(x => x.NeedsMetadata("track")).Returns(() => needsMetadata);
			trackMapService.Setup(x => x.SetMetadata("track", It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Callback<string, int, int, int>((n, s1, s2, s3) => needsMetadata = false);
			builder = new TrackMapBuilder(loggingFixture.LoggerFactory.CreateLogger<TrackMapBuilder>(), trackMapService.Object);
		}

		private TrackMap UnitCircle() {
			TrackMap map = new TrackMap() { Points = new List<Point2D>() };
			int length = 32;
			for(int i = 0; i < length; i++) {
				double theta = i * (2 * Math.PI / length);
				map.Points.Add(new Point2D() { X = Math.Round(Math.Cos(theta), 3), Y = Math.Round(Math.Sin(theta), 3) });
			}
			return map;
		}

		private string Sector(Point2D point, int sectorOffset) {
			int sector = ((point.X > 0 && point.Y >= 0 ? 1 : (point.X == 0 && point.Y > 0) || point.X < 0 ? 2 : 3) + sectorOffset) % 3;
			if(sector == 1)
				return "SECTOR1";
			else if(sector == 2)
				return "SECTOR2";
			else
				return "SECTOR3";
		}

		private List<List<Standing>> UnitCirclePath(int laps, bool skipEveryOther = false, int sectorOffset = 0) {
			List<List<Standing>> path = new List<List<Standing>>();
			TrackMap map = UnitCircle();
			for(int i = 0; i < laps; i++) {
				for(int j = 0; j < map.Points.Count; j++) {
					if(skipEveryOther && j % 2 == i % 2)
						continue;
					path.Add(new List<Standing>() {
						new Standing() {
							slotID = 0,
							vehicleFilename = "veh",
							lapsCompleted = i,
							lastLapTime = 60,
							sector = Sector(map.Points[j], sectorOffset),
							carPosition = new Position() { x = map.Points[j].X, z = map.Points[j].Y }
						}
					});
				}
			}
			return path;
		}

		[Fact]
		public void Update_UnitCircleIdenticalUnitCirclePaths_DeterminesSectorMarkers() {
			foreach(List<Standing> standings in UnitCirclePath(12))
				builder.Update("s1", "track", standings);
			trackMapService.Verify(x => x.SetMetadata("track", 0, 8, 24), Times.Once);
		}

		[Fact]
		public void Update_UnitCircleIdenticalUnitCirclePathsStartInS2_DeterminesSectorMarkers() {
			foreach(List<Standing> standings in UnitCirclePath(12, sectorOffset: 1))
				builder.Update("s1", "track", standings);
			trackMapService.Verify(x => x.SetMetadata("track", 24, 0, 8), Times.Once);
		}

		[Fact]
		public void Update_UnitCircleIdenticalUnitCirclePathsStartInS3_DeterminesSectorMarkers() {
			foreach(List<Standing> standings in UnitCirclePath(12, sectorOffset: 2))
				builder.Update("s1", "track", standings);
			trackMapService.Verify(x => x.SetMetadata("track", 8, 24, 0), Times.Once);
		}

		[Fact]
		public void Update_UnitCircleDistinctUnitCirclePaths_Undetermined() {
			foreach(List<Standing> standings in UnitCirclePath(12, true))
				builder.Update("s1", "track", standings);
			trackMapService.Verify(x => x.SetMetadata("track", It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
		}
	}
}
