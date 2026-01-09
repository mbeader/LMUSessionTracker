using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Hubs {
	public class SessionHub : Hub {
		public async Task SendMessage(string user, string message)
			=> await Clients.All.SendAsync("ReceiveMessage", user, message);
	}
}
