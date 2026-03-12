using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionSummary {
		public string SessionId { get; set; }
		public string PrimaryClientId { get; set; }
		public List<string> SecondaryClientIds { get; set; }
		public int ClientCount => (PrimaryClientId == null ? 0 : 1) + SecondaryClientIds.Count;
		public string Track { get; set; }
		public string Type { get; set; }
		public bool Online { get; set; }
		public DateTime Timestamp { get; set; }
		public DateTime LastUpdate { get; set; }
		public bool Finished { get; set; }
		public bool Active { get; set; }
		public int CarCount { get; set; }
		public int LapCount { get; set; }
		public double CurrentET { get; set; }
		public double Remaining { get; set; }
		public int Phase { get; set; }
	}
}
