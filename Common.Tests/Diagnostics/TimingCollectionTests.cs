using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.Diagnostics;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Tests.Diagnostics {
	public class TimingCollectionTests {
		private static readonly DateTime dt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
		private readonly TimingCollection timing;
		private readonly FakeLogger<TimingCollection> logger;

		public TimingCollectionTests(LoggingFixture loggingFixture) {
			logger = new FakeLogger<TimingCollection>(loggingFixture.LoggerFactory.CreateLogger<TimingCollection>());
			timing = new TimingCollection(logger);
		}

		private class FakeLogger<T> : ILogger<T> {
			private readonly ILogger<T> logger;

			public List<string> Messages { get; } = new List<string>();

			public FakeLogger(ILogger<T> logger) {
				this.logger = logger;
			}

			public IDisposable BeginScope<TState>(TState state) where TState : notnull {
				return logger.BeginScope(state);
			}

			public bool IsEnabled(LogLevel logLevel) {
				return logger.IsEnabled(logLevel);
			}

			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
				Messages.Add(formatter(state, exception));
				logger.Log(logLevel, eventId, state, exception, formatter);
			}
		}

		private TimeSpan TS(int ms) => new TimeSpan(ms * TimeSpan.TicksPerMillisecond);

		[Fact]
		public void Add_Initial_NoMessages() {
			timing.Add(dt, TS(10));
			AssertHelpers.Equivalent(new(), logger.Messages);
		}

		[Fact]
		public void Add_SameSecond_NoMessages() {
			timing.Add(dt, TS(10));
			timing.Add(dt.AddMilliseconds(100), TS(10));
			AssertHelpers.Equivalent(new(), logger.Messages);
		}

		[Fact]
		public void Add_NextSecond_OneMessage() {
			timing.Add(dt, TS(10));
			timing.Add(dt.AddSeconds(1), TS(10));
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
			}, logger.Messages);
		}

		[Fact]
		public void Add_NextMinute_TwoMessages() {
			timing.Add(dt, TS(10));
			timing.Add(dt.AddMinutes(1), TS(10));
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
				"12:00:00 M: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
			}, logger.Messages);
		}

		[Fact]
		public void Add_NextHour_ThreeMessages() {
			timing.Add(dt, TS(10));
			timing.Add(dt.AddHours(1), TS(10));
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
				"12:00:00 M: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
				"12:00:00 H: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
			}, logger.Messages);
		}

		[Fact]
		public void Add_NextDay_ThreeMessages() {
			timing.Add(dt, TS(10));
			timing.Add(dt.AddDays(1), TS(10));
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
				"12:00:00 M: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
				"12:00:00 H: [Count: 1, Avg: 10.000, Min: 10.000, Max: 10.000]",
			}, logger.Messages);
		}

		[Fact]
		public void Add_Series_ThreeMessages() {
			timing.Add(dt.AddMilliseconds(0), TS(10));
			timing.Add(dt.AddMilliseconds(333), TS(20));
			timing.Add(dt.AddMilliseconds(667), TS(30));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(0), TS(40));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(333), TS(50));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(667), TS(60));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(0), TS(70));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(333), TS(80));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(667), TS(90));
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 3, Avg: 20.000, Min: 10.000, Max: 30.000]",
				"12:00:30 S: [Count: 3, Avg: 50.000, Min: 40.000, Max: 60.000]",
				"12:00:00 M: [Count: 6, Avg: 35.000, Min: 10.000, Max: 60.000]",
			}, logger.Messages);
		}

		[Fact]
		public void Dump_Initial_NoMessages() {
			timing.Dump();
			AssertHelpers.Equivalent(new(), logger.Messages);
		}

		[Fact]
		public void Dump_Series_SixMessages() {
			timing.Add(dt.AddMilliseconds(0), TS(10));
			timing.Add(dt.AddMilliseconds(333), TS(20));
			timing.Add(dt.AddMilliseconds(667), TS(30));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(0), TS(40));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(333), TS(50));
			timing.Add(dt.AddSeconds(30).AddMilliseconds(667), TS(60));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(0), TS(70));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(333), TS(80));
			timing.Add(dt.AddSeconds(60).AddMilliseconds(667), TS(90));
			timing.Dump();
			AssertHelpers.Equivalent(new() {
				"12:00:00 S: [Count: 3, Avg: 20.000, Min: 10.000, Max: 30.000]",
				"12:00:30 S: [Count: 3, Avg: 50.000, Min: 40.000, Max: 60.000]",
				"12:00:00 M: [Count: 6, Avg: 35.000, Min: 10.000, Max: 60.000]",
				"12:01:00 S: [Count: 3, Avg: 80.000, Min: 70.000, Max: 90.000]",
				"12:01:00 M: [Count: 3, Avg: 80.000, Min: 70.000, Max: 90.000]",
				"12:00:00 H: [Count: 9, Avg: 50.000, Min: 10.000, Max: 90.000]",
			}, logger.Messages);
		}
	}
}
