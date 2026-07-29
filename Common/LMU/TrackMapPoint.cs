namespace LMUSessionTracker.Common.LMU {
	public class TrackMapPoint {
		public double X { get; set; }
		public double Y { get; set; } // up
		public double Z { get; set; }
		/// <summary>
		/// 0 - track
		/// 1 - pit road/paddock (including entry+exit)
		/// >=2 - grid (pair of points)
		/// >=106 - pit box (pair of points)
		/// </summary>
		public int Type { get; set; }
	}
}
