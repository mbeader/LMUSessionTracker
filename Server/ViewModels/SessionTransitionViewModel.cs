using LMUSessionTracker.Core.LMU;

namespace LMUSessionTracker.Server.ViewModels {
	public class SessionTransitionViewModel {
		public string SessionId { get; set; }
		public SessionInfo Info { get; set; }
	}
}
