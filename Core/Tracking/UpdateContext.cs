using Microsoft.Extensions.Logging;
using System;

namespace LMUSessionTracker.Core.Tracking {
	public class UpdateContext<T> {
		private readonly UpdateContextFactory factory;

		public ILogger<T> Logger { get; init; }
		public DateTime Timestamp { get; init; }
		public double CurrentET { get; init; }

		public UpdateContext(UpdateContextFactory factory) {
			this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		private UpdateContext() {
			Logger = Core.Logger.FactoryInstance.CreateLogger<T>();
		}

		public UpdateContext<TNew> Create<TNew>() {
			return factory?.Create<TNew>(Timestamp, CurrentET) ?? Create<TNew>(Timestamp, CurrentET);
		}

		public static UpdateContext<T> Create(DateTime timestamp, double currentET) {
			return Create<T>(timestamp, currentET);
		}

		public static UpdateContext<TNew> Create<TNew>(DateTime timestamp, double currentET) {
			return new UpdateContext<TNew>() { Timestamp = timestamp, CurrentET = currentET };
		}
	}
}
