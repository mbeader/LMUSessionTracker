using LMUSessionTracker.Core.Services;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Hubs;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Services {
	public class SignalRPublisherService : PublisherService {
		private readonly IHubContext<SessionHub> hubContext;
		private readonly SignalRGroupCollection groupCollection;
		private readonly DateTimeProvider dateTime;

		public SignalRPublisherService(IHubContext<SessionHub> hubContext, SignalRGroupCollection groupCollection, DateTimeProvider dateTime) {
			this.hubContext = hubContext;
			this.groupCollection = groupCollection;
			this.dateTime = dateTime;
		}

		public async Task Session(Session session, bool includeBests) {
			SessionViewModel vm = new SessionViewModel() { Info = session.LastInfo };
			vm.SetSession(session, includeBests);
			await SendLive(session, vm);
			await SendChat(session, vm);
			await SendLaps(session, vm);
		}

		private async Task SendLive(Session session, SessionViewModel vm) {
			string liveGroup = SessionHub.LiveGroup(session.SessionId);
			groupCollection.Groups.AddOrUpdate(liveGroup, session.LastUpdate, (key, value) => session.LastUpdate);
			await hubContext.Clients.Group(liveGroup).SendAsync("Live", vm);
		}

		private async Task SendChat(Session session, SessionViewModel vm) {
			if(session.Chat.NewMessages.Count > 0) {
				string chatGroup = SessionHub.ChatGroup(session.SessionId, false);
				groupCollection.Groups.AddOrUpdate(chatGroup, session.LastUpdate, (key, value) => session.LastUpdate);
				await hubContext.Clients.Group(chatGroup).SendAsync("Chat", new ChatViewModel(session.Chat.NewMessages) { Append = true });
			}
			if(session.Chat.Chat.Count > 0) {
				string chatGroup = SessionHub.ChatGroup(session.SessionId, true);
				groupCollection.Groups.AddOrUpdate(chatGroup, session.LastUpdate, (key, value) => session.LastUpdate);
				await hubContext.Clients.Group(chatGroup).SendAsync("Chat", new ChatViewModel(session.Chat.Chat));
				// switch groups
			}
		}

		private async Task SendLaps(Session session, SessionViewModel vm) {
			foreach(CarHistory car in vm.History) {
				string group = SessionHub.LapsGroup(session.SessionId, car.Key.Id());
				groupCollection.Groups.AddOrUpdate(group, session.LastUpdate, (key, value) => session.LastUpdate);
				await hubContext.Clients.Group(group).SendAsync("Laps", new LapsViewModel() { Car = car, Bests = session.Bests, CurrentET = session.LastInfo.currentEventTime });
			}
		}

		public async Task Transition(Session session, string prevSessionId) {
			SessionTransitionViewModel vm = new SessionTransitionViewModel() { SessionId = session.SessionId, Info = session.LastInfo };
			string liveGroup = SessionHub.LiveGroup(prevSessionId);
			await hubContext.Clients.Group(liveGroup).SendAsync("Transition", vm);
		}

		public async Task Sessions(List<SessionSummary> sessions) {
			await hubContext.Clients.Group(SessionHub.SessionsGroup()).SendAsync("Sessions", sessions);
		}

		public async Task Prune(DateTime now, ICollection<string> sessionIds) {
			HashSet<string> set = new HashSet<string>(sessionIds);
			foreach(var group in groupCollection.Groups) {
				if(group.Key == SessionHub.SessionsGroup())
					continue;
				string sessionId = group.Key[0..group.Key.IndexOf('-')];
				if(!set.Contains(sessionId)) {
					await hubContext.Clients.Group(group.Key).SendAsync("Kicked");
					groupCollection.Groups.TryRemove(group.Key, out DateTime _);
				}
			}
		}
	}
}
