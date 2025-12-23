using System.Collections.Generic;

namespace LMUSessionTracker.TestClient {
	public class TestClientOptions {
		public int NumClients { get; set; } = 1;
		public bool UseReplay { get; set; } = true;
		public bool SingleReplay { get; set; } = true;
		public List<string> ReplayDirectories { get; set; } = new List<string>();
	}
}
