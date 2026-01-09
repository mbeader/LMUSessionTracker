using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Hubs {
	public class SessionHub : Hub {
		private static readonly ConcurrentDictionary<string, bool> groups = new ConcurrentDictionary<string, bool>();

		public async Task Join(string sessionId, string type) {
			if(string.IsNullOrEmpty(sessionId))
				return;
			switch(type) {
				case "live":
					break;
				default:
					return;
			}
			string group = Group(sessionId, type);
			groups.TryAdd(group, true);
			await Groups.AddToGroupAsync(Context.ConnectionId, group);
			await Clients.Caller.SendAsync("Joined", sessionId, type);
		}

		public async Task Leave() {
			// TODO: work around microsoft's garbage
			foreach(string group in groups.Keys)
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
		}

		public static string LiveGroup(string sessionId) => Group(sessionId, "live");
		private static string Group(string sessionId, string type) => $"{sessionId}-{type}";
	}
}
