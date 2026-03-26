using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.Diagnostics;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Tests.Diagnostics {
	public class TimingAggregateTests {
		private static readonly DateTime dt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

		public TimingAggregateTests(LoggingFixture loggingFixture) {
		}

		private TimeSpan TS(int ms) => new TimeSpan(ms * TimeSpan.TicksPerMillisecond);

		[Fact]
		public void Construct_NullDurations() {
			Assert.Equivalent(new TimingAggregate(), new TimingAggregate(null, dt, TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_EmptyDurations() {
			Assert.Equivalent(new TimingAggregate(), new TimingAggregate(new(), dt, TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_SingleDurationSecond() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt.AddSeconds(61), Type = TimingAggregateType.Second, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { TS(10) }, dt.AddMilliseconds(61001), TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_SingleDurationMinute() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt.AddMinutes(1), Type = TimingAggregateType.Minute, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { TS(10) }, dt.AddMilliseconds(61001), TimingAggregateType.Minute));
		}

		[Fact]
		public void Construct_SingleDurationHour() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt, Type = TimingAggregateType.Hour, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { TS(10) }, dt.AddMilliseconds(61001), TimingAggregateType.Hour));
		}

		[Fact]
		public void Construct_NullAggregates() {
			Assert.Equivalent(new TimingAggregate(), new TimingAggregate(null, TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_EmptyAggregates() {
			Assert.Equivalent(new TimingAggregate(), new TimingAggregate(new(), TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_SingleAggregateEmpty() {
			Assert.Equivalent(new TimingAggregate(), new TimingAggregate(new() { new() }, TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_SingleAggregateSecond() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt.AddSeconds(61), Type = TimingAggregateType.Second, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { new() { Timestamp = dt.AddMilliseconds(61001), Count = 1, Average = 10, Max = 10, Min = 10 } }, TimingAggregateType.Second));
		}

		[Fact]
		public void Construct_SingleAggregateMinute() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt.AddMinutes(1), Type = TimingAggregateType.Minute, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { new() { Timestamp = dt.AddMilliseconds(61001), Count = 1, Average = 10, Max = 10, Min = 10 } }, TimingAggregateType.Minute));
		}

		[Fact]
		public void Construct_SingleAggregateHour() {
			Assert.Equivalent(new TimingAggregate() { Timestamp = dt, Type = TimingAggregateType.Hour, Count = 1, Average = 10, Max = 10, Min = 10 },
				new TimingAggregate(new() { new() { Timestamp = dt.AddMilliseconds(61001), Count = 1, Average = 10, Max = 10, Min = 10 } }, TimingAggregateType.Hour));
		}
	}
}
