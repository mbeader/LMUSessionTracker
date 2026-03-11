using LMUSessionTracker.Core.Services;
using LMUSessionTracker.CoreServer.Services;
using LMUSessionTracker.CoreServer.Tracking;
using LMUSessionTracker.Server.Hubs;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Services {
	public class SignalRPublisherService : PublisherService {
		private readonly ILogger<SignalRPublisherService> logger;
		private readonly IHubContext<SessionHub> hubContext;
		private readonly SignalRGroupCollection groupCollection;
		private readonly DateTimeProvider dateTime;

		public SignalRPublisherService(ILogger<SignalRPublisherService> logger, IHubContext<SessionHub> hubContext, SignalRGroupCollection groupCollection, DateTimeProvider dateTime) {
			this.logger = logger;
			this.hubContext = hubContext;
			this.groupCollection = groupCollection;
			this.dateTime = dateTime;
		}

		public async Task Session(Session session, bool includeBests) {
			try {
				SessionViewModel vm = new SessionViewModel() { Info = session.LastInfo };
				vm.SetSession(session, includeBests);
				await SendLive(session, vm);
				await SendChat(session, vm);
				await SendLaps(session, vm);
			} catch(Exception e) {
				logger.LogError(e, $"Failed to publish session");
			}
		}

		private async Task SendLive(Session session, SessionViewModel vm) {
			string liveGroup = SessionHub.LiveGroup(session.SessionId);
			groupCollection.AddOrUpdateGroup(liveGroup, session.LastUpdate);
			await hubContext.Clients.Group(liveGroup).SendAsync("Live", vm);
		}

		private async Task SendChat(Session session, SessionViewModel vm) {
			string chatGroup = SessionHub.ChatGroup(session.SessionId, false);
			string chatGroupRefresh = SessionHub.ChatGroup(session.SessionId, true);
			if(session.Chat.NewMessages.Count > 0 && groupCollection.GetConnections(chatGroup).Count > 0) {
				groupCollection.AddOrUpdateGroup(chatGroup, session.LastUpdate);
				await hubContext.Clients.Group(chatGroup).SendAsync("Chat", new ChatViewModel(session.Chat.NewMessages) { Append = true });
			}
			if(session.Chat.Chat.Count > 0 && groupCollection.GetConnections(chatGroupRefresh).Count > 0) {
				groupCollection.AddOrUpdateGroup(chatGroupRefresh, session.LastUpdate);
				await hubContext.Clients.Group(chatGroupRefresh).SendAsync("Chat", new ChatViewModel(session.Chat.Chat));
				groupCollection.AddOrUpdateGroup(chatGroup, session.LastUpdate);
				foreach(string connection in groupCollection.SwapConnectionGroups(chatGroupRefresh, chatGroup)) {
					await hubContext.Groups.RemoveFromGroupAsync(connection, chatGroupRefresh);
					await hubContext.Groups.AddToGroupAsync(connection, chatGroup);
				}
			}
		}

		private async Task SendLaps(Session session, SessionViewModel vm) {
			foreach(CarHistory car in vm.History) {
				string group = SessionHub.LapsGroup(session.SessionId, car.Key.Id());
				groupCollection.AddOrUpdateGroup(group, session.LastUpdate);
				await hubContext.Clients.Group(group).SendAsync("Laps", new LapsViewModel() { Car = car, Bests = session.Bests, CurrentET = session.LastInfo.currentEventTime });
			}
		}

		public async Task Transition(Session session, string prevSessionId) {
			try {
				SessionTransitionViewModel vm = new SessionTransitionViewModel() { SessionId = session.SessionId, Info = session.LastInfo };
				string liveGroup = SessionHub.LiveGroup(prevSessionId);
				await hubContext.Clients.Group(liveGroup).SendAsync("Transition", vm);
			} catch(Exception e) {
				logger.LogError(e, $"Failed to publish transition");
			}
		}

		public async Task Sessions(List<SessionSummary> sessions) {
			try {
				await hubContext.Clients.Group(SessionHub.SessionsGroup()).SendAsync("Sessions", sessions);
			} catch(Exception e) {
				logger.LogError(e, $"Failed to publish sessions");
			}
		}

		public async Task Prune(DateTime now, ICollection<string> sessionIds) {
			HashSet<string> set = new HashSet<string>(sessionIds);
			foreach(var group in groupCollection.Groups) {
				if(group == SessionHub.SessionsGroup())
					continue;
				string sessionId = group[0..group.IndexOf('-')];
				if(!set.Contains(sessionId)) {
					await hubContext.Clients.Group(group).SendAsync("Kicked");
					groupCollection.RemoveGroup(group);
				}
			}
		}
	}
}
