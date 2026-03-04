using LMUSessionTracker.Server.Models;
using System;

namespace LMUSessionTracker.Server.ViewModels {
	public class ChatMessage {
		public long Timestamp { get; set; }
		public string Message { get; set; }

		public ChatMessage(Chat chat = null) {
			if(chat != null) {
				Timestamp = (long)(chat.Timestamp - DateTime.UnixEpoch).TotalMilliseconds;
				Message = chat.Message;
			}
		}
	}
}
