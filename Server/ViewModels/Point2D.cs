using LMUSessionTracker.Core.LMU;

namespace LMUSessionTracker.Server.ViewModels {
	public class Point2D {
		public double X { get; set; }
		public double Y { get; set; }

		public Point2D() { }

		public Point2D(TrackMapPoint points) {
			X = points.X;
			Y = points.Z;
		}
	}
}
