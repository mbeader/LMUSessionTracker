using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Diagnostics {
	public readonly struct TimingAggregate {
		public DateTime Timestamp { get; init; }
		public TimingAggregateType Type { get; init; }
		public int Count { get; init; }
		public double Average { get; init; }
		public double Max { get; init; }
		public double Min { get; init; }

		public TimingAggregate() { }

		public TimingAggregate(List<TimeSpan> durations, DateTime timestamp, TimingAggregateType type) {
			if(durations == null || durations.Count == 0)
				return;
			double max = double.NegativeInfinity;
			double min = double.PositiveInfinity;
			double total = 0;
			foreach(TimeSpan duration in durations) {
				double ms = duration.TotalMilliseconds;
				if(ms > max)
					max = ms;
				if(ms < min)
					min = ms;
				total += ms;
			}
			Max = max;
			Min = min;
			Average = total / durations.Count;
			Count = durations.Count;
			Timestamp = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, type == TimingAggregateType.Minute || type == TimingAggregateType.Second ? timestamp.Minute : 0, type == TimingAggregateType.Second ? timestamp.Second : 0);
			Type = type;
		}

		public TimingAggregate(List<TimingAggregate> aggregates, TimingAggregateType type) {
			if(aggregates == null || aggregates.Count == 0)
				return;
			double max = double.NegativeInfinity;
			double min = double.PositiveInfinity;
			double total = 0;
			int count = 0;
			foreach(TimingAggregate aggregate in aggregates) {
				if(aggregate.Count == 0)
					continue;
				if(aggregate.Max > max)
					max = aggregate.Max;
				if(aggregate.Min < min)
					min = aggregate.Min;
				total += aggregate.Average * aggregate.Count;
				count += aggregate.Count;
			}
			if(count == 0)
				return;
			Max = max;
			Min = min;
			Average = total / count;
			Count = count;
			TimingAggregate first = aggregates[0];
			Timestamp = new DateTime(first.Timestamp.Year, first.Timestamp.Month, first.Timestamp.Day, first.Timestamp.Hour, type == TimingAggregateType.Minute || type == TimingAggregateType.Second ? first.Timestamp.Minute : 0, type == TimingAggregateType.Second ? first.Timestamp.Second : 0);
			Type = type;
		}
	}
}
