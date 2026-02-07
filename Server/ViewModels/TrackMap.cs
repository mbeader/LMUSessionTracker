using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class TrackMap {
		public List<Point2D> Points { get; set; }
		public List<Point2D> Pits { get; set; }
		public List<Point2D> S1 { get; set; }
		public List<Point2D> S2 { get; set; }
		public List<Point2D> S3 { get; set; }
		public double MaxX { get; set; }
		public double MaxY { get; set; }
		public double MinX { get; set; }
		public double MinY { get; set; }
	}
}
