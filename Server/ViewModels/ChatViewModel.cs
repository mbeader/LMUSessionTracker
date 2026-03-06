using LMUSessionTracker.Server.Models;
using System.Collections.Generic;
using CoreChat = LMUSessionTracker.Core.LMU.Chat;

namespace LMUSessionTracker.Server.ViewModels {
	public class ChatViewModel {
		public List<ChatMessage> Chat { get; set; }
		public bool Append { get; set; }

		public ChatViewModel(List<Chat> chat = null) {
			if(chat != null)
				Chat = chat.ConvertAll(x => new ChatMessage(x));
			else
				Chat = new List<ChatMessage>();
		}

		public ChatViewModel(List<CoreChat> chat) {
			if(chat != null)
				Chat = chat.ConvertAll(x => new ChatMessage(x));
			else
				Chat = new List<ChatMessage>();
		}
	}
}
