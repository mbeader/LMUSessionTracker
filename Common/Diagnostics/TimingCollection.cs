using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Diagnostics {
	public class TimingCollection {
		private readonly List<TimeSpan> runsSecond = new List<TimeSpan>();
		private readonly List<TimingAggregate> runsMinute = new List<TimingAggregate>();
		private readonly List<TimingAggregate> runsHour = new List<TimingAggregate>();
		private readonly ILogger<TimingCollection> logger;
		private DateTime lastTimestamp = DateTime.MinValue;

		public TimingCollection(ILogger<TimingCollection> logger) {
			this.logger = logger;
		}

		public void Add(DateTime timestamp, TimeSpan duration) {
			List<TimingAggregate> newAggregates = new List<TimingAggregate>();
			if(lastTimestamp != DateTime.MinValue) {
				bool sameDate = lastTimestamp.Date == timestamp.Date;
				bool sameHour = lastTimestamp.Hour == timestamp.Hour;
				bool sameMinute = lastTimestamp.Minute == timestamp.Minute;
				bool sameSecond = lastTimestamp.Second == timestamp.Second;
				if(!sameDate || !sameHour || !sameMinute || !sameSecond) {
					TimingAggregate newSecond = new TimingAggregate(runsSecond, lastTimestamp, TimingAggregateType.Second);
					newAggregates.Add(newSecond);
					runsMinute.Add(newSecond);
					runsSecond.Clear();
				}
				if(!sameDate || !sameHour || !sameMinute) {
					TimingAggregate newMinute = new TimingAggregate(runsMinute, TimingAggregateType.Minute);
					newAggregates.Add(newMinute);
					runsHour.Add(newMinute);
					runsMinute.Clear();
				}
				if(!sameDate || !sameHour) {
					TimingAggregate newHour = new TimingAggregate(runsHour, TimingAggregateType.Hour);
					newAggregates.Add(newHour);
					runsHour.Clear();
				}
			}
			runsSecond.Add(duration);
			lastTimestamp = timestamp;
			foreach(TimingAggregate a in newAggregates)
				Log(a);
		}

		private void Log(TimingAggregate a) {
			if(a.Count > 0)
				logger.LogInformation($"{a.Timestamp:HH:mm:ss} {a.Type.ToString()[0]}: [Count: {a.Count}, Avg: {a.Average:N3}, Min: {a.Min:N3}, Max: {a.Max:N3}]");
		}

		public void Dump() {
			List<TimingAggregate> newAggregates = new List<TimingAggregate>();
			TimingAggregate newSecond = new TimingAggregate(runsSecond, lastTimestamp, TimingAggregateType.Second);
			newAggregates.Add(newSecond);
			runsMinute.Add(newSecond);
			runsSecond.Clear();
			TimingAggregate newMinute = new TimingAggregate(runsMinute, TimingAggregateType.Minute);
			newAggregates.Add(newMinute);
			runsHour.Add(newMinute);
			runsMinute.Clear();
			TimingAggregate newHour = new TimingAggregate(runsHour, TimingAggregateType.Hour);
			newAggregates.Add(newHour);
			runsHour.Clear();
			lastTimestamp = DateTime.MinValue;
			foreach(TimingAggregate a in newAggregates)
				Log(a);
		}
	}
}
