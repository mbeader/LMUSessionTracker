using LMUSessionTracker.Common.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class ChatCollection {
		public List<Chat> Chat { get; private init; } = new List<Chat>();
		public List<Chat> NewMessages { get; private init; } = new List<Chat>();
		public Chat LastChat => Chat.Count > 0 ? Chat[^1] : null;

		public ChatCollection(List<Chat> chat = null) {
			if(chat != null)
				Chat.AddRange(chat);
		}

		public void Update(List<Chat> chat) {
			NewMessages.Clear();
			if(chat != null) {
				foreach(Chat c in chat) {
					if(!Chat.Exists(x => IsSameChat(c, x))) {
						if(LastChat != null && LastChat.timestamp > c.timestamp)
							continue;
						Chat.Add(c);
						NewMessages.Add(c);
					}
				}
			}
		}

		private static bool IsSameChat(Chat a, Chat b) {
			return a.timestamp == b.timestamp && a.message == b.message;
		}
	}
}
