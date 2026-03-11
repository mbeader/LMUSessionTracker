using System;

namespace LMUSessionTracker.CoreServer.Services {
	public interface UuidVersion7Provider {
		public Guid CreateVersion7(DateTime timestamp);
	}

	public class DefaultUuidVersion7Provider : UuidVersion7Provider {
		public Guid CreateVersion7(DateTime timestamp) {
			return GuidHelpers.CreateVersion7(new DateTimeOffset(timestamp, TimeSpan.Zero));
		}
	}
}
