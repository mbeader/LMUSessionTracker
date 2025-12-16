using System;

namespace LMUSessionTracker.Core.Services {
	public interface DateTimeProvider {
		public DateTime UtcNow { get; }
	}

	public class DefaultDateTimeProvider : DateTimeProvider {
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
