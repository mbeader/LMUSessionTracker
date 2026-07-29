using LMUSessionTracker.Common.LMU;
using System;

namespace LMUSessionTracker.Server.ViewModels {
	public class Point2D {
		public double X { get; set; }
		public double Y { get; set; }

		public Point2D() { }

		public Point2D(TrackMapPoint points) {
			X = points.X;
			Y = points.Z;
		}

		public static double Distance(Point2D a, Point2D b) {
			return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
		}
	}
}
