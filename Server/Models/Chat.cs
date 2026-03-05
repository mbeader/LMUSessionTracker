using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	[Index(nameof(SessionId), nameof(Timestamp), nameof(Nanoseconds), nameof(Message), IsUnique = true)]
	public class Chat {
		private const long K = 1000;
        private const long NanosecondsPerTick = 100;
        private const long TicksPerMicrosecond = 10;
        private const long TicksPerMillisecond = TicksPerMicrosecond * K; // 10,000
        private const long TicksPerSecond = TicksPerMillisecond * K; // 10,000,000
		private const long NanosecondsPerMillisecond = K * K; // 1,000,000

		[Key, Required]
		public long ChatId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }

		public string Message { get; set; }
		/// <summary>
		/// Unix seconds
		/// </summary>
		public long Timestamp { get; set; }
		/// <summary>
		/// Sub-second portion of Unix timestamp, in nanoseconds
		/// </summary>
		public long Nanoseconds { get; set; }

		public Session Session { get; set; }

		public long UnixMilliseconds => Timestamp * K + Nanoseconds / NanosecondsPerMillisecond;
		public DateTime DateTime => new DateTime(Timestamp * K + Nanoseconds / NanosecondsPerTick);

		public void From(Core.LMU.Chat chat) {
			Message = chat.message;
			Timestamp = (long)(chat.timestamp / TicksPerSecond);
			Nanoseconds = ((long)chat.timestamp - Timestamp * TicksPerSecond)*NanosecondsPerTick;
		}

		public Core.LMU.Chat To() {
			return new Core.LMU.Chat() {
				message = Message,
				timestamp = (ulong)(Timestamp * TicksPerSecond + Nanoseconds)
			};
		}
	}
}
