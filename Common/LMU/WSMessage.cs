using System.Collections.Generic;

namespace LMUSessionTracker.Common.LMU {
	public interface WSMessage<T> {
		public string messageType { get; set; }
		public string topic { get; set; }
		public T body { get; set; }
	}

	public class WSMessageLiveStandings : WSMessage<List<WSStanding>> {
		public string messageType { get; set; }
		public string topic { get; set; }
		public List<WSStanding> body { get; set; }
	}

	public class WSMessageSessionInfo : WSMessage<WSSessionInfo> {
		public string messageType { get; set; }
		public string topic { get; set; }
		public WSSessionInfo body { get; set; }
	}
}
