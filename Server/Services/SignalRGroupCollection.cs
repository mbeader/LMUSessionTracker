using System;
using System.Collections.Concurrent;

namespace LMUSessionTracker.Server.Services {
	public class SignalRGroupCollection {
		public ConcurrentDictionary<string, DateTime> Groups { get; } = new ConcurrentDictionary<string, DateTime>();
	}
}
