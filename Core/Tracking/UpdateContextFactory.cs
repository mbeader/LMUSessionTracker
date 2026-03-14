using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class UpdateContextFactory {
		private readonly Dictionary<Type, ILogger> loggers = new Dictionary<Type, ILogger>();
		private readonly ILoggerFactory loggerFactory;

		public UpdateContextFactory(ILoggerFactory loggerFactory) {
			this.loggerFactory = loggerFactory;
		}

		public UpdateContext<T> Create<T>(DateTime timestamp, double currentET) {
			if(!loggers.TryGetValue(typeof(T), out ILogger logger)) {
				logger = loggerFactory.CreateLogger<T>();
				loggers.Add(typeof(T), logger);
			}
			return new UpdateContext<T>(this) { Logger = (ILogger<T>)logger, Timestamp = timestamp, CurrentET = currentET };
		}
	}
}
