using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Server.Services;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Hubs {
	public class SessionHub : Hub {
		private readonly SignalRGroupCollection groupCollection;

		public SessionHub(SignalRGroupCollection groupCollection) {
			this.groupCollection = groupCollection;
		}

		public async Task Join(JoinRequest request) {
			string group;
			if(string.IsNullOrEmpty(request.SessionId)) {
				switch(request.Type) {
					case "sessions":
						group = SessionsGroup();
						break;
					default:
						return;
				}
			} else {
				switch(request.Type) {
					case "live":
					case "laps":
					case "chat":
						break;
					default:
						return;
				}
				group = Group(request);
			}
			groupCollection.AddOrUpdateGroup(group, DateTime.UnixEpoch);
			groupCollection.AddConnectionToGroup(group, Context.ConnectionId);
			await Groups.AddToGroupAsync(Context.ConnectionId, group);
			await Clients.Caller.SendAsync("Joined", request);
		}

		public async Task Leave() {
			foreach(string group in groupCollection.Groups)
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
			groupCollection.RemoveConnectionFromAllGroups(Context.ConnectionId);
		}

		public static string SessionsGroup() => "sessions";
		public static string LiveGroup(string sessionId) => Group(sessionId, "live");
		public static string LapsGroup(string sessionId, string carId) => Group(sessionId, "laps", carId);
		public static string ChatGroup(string sessionId, bool refresh) => Group(sessionId, "chat", refresh: refresh);
		private static string Group(JoinRequest request) => Group(request.SessionId, request.Type, request.Key, request.Refresh);

		/// <summary>
		/// Produces group names of the forms:
		/// sessionId-type
		/// sessionId-type[refresh]
		/// sessionId-type-key
		/// sessionId-type[refresh]-key
		/// </summary>
		private static string Group(string sessionId, string type, string key = null, bool refresh = false) {
			string keyPart = key != null ? $"-{key}" : string.Empty;
			string refreshPart = refresh ? "[refresh]" : string.Empty;
			return $"{sessionId}-{type}{refreshPart}{keyPart}";
		}
	}
}
