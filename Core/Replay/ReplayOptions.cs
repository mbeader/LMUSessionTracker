namespace LMUSessionTracker.Core.Replay {
	public class ReplayOptions {
		public string Directory { get; set; }
		public bool ValidateResponses { get; set; } = false;
		public int Interval { get; set; } = 0;
	}
}