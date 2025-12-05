namespace LMUSessionTracker.Core.LMU {
	public class TrackMapPoint {
		public double X { get; set; }
		public double Y { get; set; } // up
		public double Z { get; set; }
		/// <summary>
		/// 0 - track
		/// 1 - pit/paddock (including entry+exit)
		/// >=3 - grid (pair of points)
		/// >=107 - pit (pair of points)
		/// </summary>
		public int Type { get; set; }
	}
}
