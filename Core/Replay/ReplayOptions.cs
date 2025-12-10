namespace LMUSessionTracker.Core.Replay {
	public class ReplayOptions {
		public string Directory { get; set; }
		public bool ValidateResponses { get; set; } = false;
		public int Delay { get; set; } = 0;
	}
}