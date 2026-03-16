namespace LMUSessionTracker.Common.Services {
	public interface IntervalProvider {
		public int GetInterval();
	}

	public class DefaultIntervalProvider : IntervalProvider {
		protected const int defaultInterval = 1000;
		private readonly int interval;

		public DefaultIntervalProvider(ClientInfo client) {
			interval = client.OverrideInterval && client.Interval.HasValue ? client.Interval.Value : defaultInterval;
		}

		public virtual int GetInterval() {
			return interval;
		}
	}
}
