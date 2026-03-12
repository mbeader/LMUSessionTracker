using LMUSessionTracker.Server.Models;
using CoreChat = LMUSessionTracker.Common.LMU.Chat;

namespace LMUSessionTracker.Server.ViewModels {
	public class ChatMessage {
		public long Timestamp { get; set; }
		public string Message { get; set; }

		public ChatMessage(Chat chat = null) {
			if(chat != null) {
				Timestamp = chat.UnixMilliseconds;
				Message = chat.Message;
			}
		}

		public ChatMessage(CoreChat chat) {
			if(chat != null) {
				Timestamp = (long)(chat.timestamp / 10000);
				Message = chat.message;
			}
		}
	}
}
