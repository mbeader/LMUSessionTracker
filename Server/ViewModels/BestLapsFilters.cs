using LMUSessionTracker.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMUSessionTracker.Server.ViewModels {
	public class BestLapsFilters {
		private static readonly List<string> practiceSessions = new List<string>() { "TESTDAY", "PRACTICE1", "PRACTICE2", "PRACTICE3", "PRACTICE4", "WARMUP" };
		private static readonly List<string> qualifyingSessions = new List<string>() { "QUALIFY1", "QUALIFY2", "QUALIFY3", "QUALIFY4" };
		private static readonly List<string> raceSessions = new List<string>() { "RACE1", "RACE2", "RACE3", "RACE4" };

		[Required]
		public string Track { get; set; }
		public DateTime? Since { get; set; }
		public string Network { get; set; }
		public List<string> Classes { get; set; }
		public List<string> Sessions { get; set; }
		public bool KnownDriversOnly { get; set; }

		public bool? OnlineOnly => Network == "online" ? true : Network == "offline" ? false : null;
		public List<string> SessionTypes => GetSessionType();

		public List<string> GetSessionType() {
			List<string> sessions = new List<string>();
			if(Sessions == null || Sessions.Contains("Practice"))
				sessions.AddRange(practiceSessions);
			if(Sessions == null || Sessions.Contains("Qualifying"))
				sessions.AddRange(qualifyingSessions);
			if(Sessions == null || Sessions.Contains("Race"))
				sessions.AddRange(raceSessions);
			return sessions;
		}
	}

	public class BestLapsViewModel {
		public List<BestLap> Laps { get; set; }
		public Dictionary<string, ClassBest> ClassBests { get; set; }
	}
}
