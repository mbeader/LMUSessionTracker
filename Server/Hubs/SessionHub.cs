using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Hubs {
	public class SessionHub : Hub {
		private static readonly ConcurrentDictionary<string, bool> groups = new ConcurrentDictionary<string, bool>();

		public async Task Join(JoinRequest request) {
			if(string.IsNullOrEmpty(request.SessionId))
				return;
			switch(request.Type) {
				case "live":
				case "laps":
					break;
				default:
					return;
			}
			string group = Group(request);
			groups.TryAdd(group, true);
			await Groups.AddToGroupAsync(Context.ConnectionId, group);
			await Clients.Caller.SendAsync("Joined", request);
		}

		public async Task Leave() {
			// TODO: work around microsoft's garbage
			foreach(string group in groups.Keys)
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
		}

		public static string LiveGroup(string sessionId) => Group(sessionId, "live");
		public static string LapsGroup(string sessionId, string carId) => Group(sessionId, "laps", carId);
		private static string Group(JoinRequest request) => Group(request.SessionId, request.Type, request.Key);
		private static string Group(string sessionId, string type, string key = null) => $"{sessionId}-{type}{(key != null ? $"-{key}" : string.Empty)}";
	}
}
