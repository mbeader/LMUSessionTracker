using LMUSessionTracker.Core.LMU;
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
				Chat.AddRange(chat);
				NewMessages.AddRange(chat);
			}
		}
	}
}
