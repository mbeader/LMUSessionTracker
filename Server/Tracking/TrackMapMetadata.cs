namespace LMUSessionTracker.Server.Tracking {
	public class TrackMapMetadata {
		public int S1Index { get; set; }
		public int S2Index { get; set; }
		public int S3Index { get; set; }

		public TrackMapMetadata() { }

		public TrackMapMetadata(int s1, int s2, int s3) {
			S1Index = s1;
			S2Index = s2;
			S3Index = s3;
		}
	}
}
