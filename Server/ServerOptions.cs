namespace LMUSessionTracker.Server {
	public class ServerOptions {
		public bool UseLocalClient { get; set; } = false;
		public bool RejectAllClients { get; set; } = false;
		public bool RecreateDatabaseOnStartup { get; set; } = false;
		public bool UseHttpsRedirection { get; set; } = true;
	}
}
