using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Hubs;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Services {
	public class SignalRPublisherService : PublisherService {
		private readonly IHubContext<SessionHub> hubContext;

		public SignalRPublisherService(IHubContext<SessionHub> hubContext) {
			this.hubContext = hubContext;
		}

		public async Task Session(Session session) {
			SessionViewModel vm = new SessionViewModel() { Info = session.LastInfo };
			vm.SetSession(session);
			await hubContext.Clients.Group(SessionHub.LiveGroup(session.SessionId)).SendAsync("Live", vm);
			foreach(CarHistory car in vm.History) {
				string group = SessionHub.LapsGroup(session.SessionId, car.Key.Id());
				await hubContext.Clients.Group(group).SendAsync("Laps", new LapsViewModel() { Car = car });
			}
		}
	}
}
