using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Services {
	public class SignalRPublisherService : PublisherService {
		private readonly IHubContext<SessionHub> hubContext;

		public SignalRPublisherService(IHubContext<SessionHub> hubContext) {
			this.hubContext = hubContext;
		}

		public async Task Session(Session session) {
			await hubContext.Clients.All.SendAsync("ReceiveMessage", "u", $"{session.SessionId} {session.LastInfo.timeRemainingInGamePhase}");
		}
	}
}
