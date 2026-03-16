using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.Protocol;
using LMUSessionTracker.Common.Services;
using System.Linq;

namespace LMUSessionTracker.Common.Replay {
	public class ReplayClientIntervalProvider : DefaultIntervalProvider, ClientIntervalProvider {
		private int nextInterval = -1;

		public ReplayClientIntervalProvider(ClientInfo client) : base(client) {
		}

		public override int GetInterval() {
			return nextInterval >= 0 ? nextInterval : base.GetInterval();
		}

		public void SetInterval(ClientHandler handler, ProtocolMessage message) {
			int interval = -1;
			if(handler.SessionId != null && handler.Role == ProtocolRole.Primary && handler.State == ClientState.Working) {
				//if(message.SessionInfo?.session == "RACE1" && (message.Standings?.Max(x => x.lapsCompleted >= 30) ?? false))
				//	interval = 500;
			}
			nextInterval = interval;
		}
	}
}
