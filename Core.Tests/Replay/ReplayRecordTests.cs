using LMUSessionTracker.Core.Replay;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMUSessionTracker.Core.Tests.Replay {
	public class ReplayRecordTests {

		public ReplayRecordTests() {
		}

		public class FakeIntRecord : IntRecord {
			public new bool Null { get { return base.Null; } set { base.Null = value; } }
			public new bool MaxValue { get { return base.MaxValue; } set { base.MaxValue = value; } }
			public new bool MinValue { get { return base.MinValue; } set { base.MinValue = value; } }
			public new bool Zero { get { return base.Zero; } set { base.Zero = value; } }
			public new bool NegativeOne { get { return base.NegativeOne; } set { base.NegativeOne = value; } }
			public new int? MaxPositive { get { return base.MaxPositive; } set { base.MaxPositive = value; } }
			public new int? MinPositive { get { return base.MinPositive; } set { base.MinPositive = value; } }
			public new int? MaxNegative { get { return base.MaxNegative; } set { base.MaxNegative = value; } }
			public new int? MinNegative { get { return base.MinNegative; } set { base.MinNegative = value; } }
		}

		private ReplayRecord Int(int? value) {
			ReplayRecord r = new IntRecord();
			r.Add(value);
			return r;
		}

		[Fact]
		public void Add_NullInt_NullNoStats() {
			Assert.Equivalent(new FakeIntRecord() { Null = true }, Int(null));
		}

		[Fact]
		public void Add_Zero_ZeroNoStats() {
			Assert.Equivalent(new FakeIntRecord() { Zero = true }, Int(0));
		}

		[Fact]
		public void Add_NegativeOne_NegativeOneNegativeStats() {
			Assert.Equivalent(new FakeIntRecord() { NegativeOne = true, MinNegative = -1, MaxNegative = -1 }, Int(-1));
		}

		[Fact]
		public void Add_Positive_PositiveStats() {
			ReplayRecord record = Int(3);
			Assert.Equivalent(new FakeIntRecord() { MinPositive = 3, MaxPositive = 3 }, record);
			record.Add(2);
			Assert.Equivalent(new FakeIntRecord() { MinPositive = 2, MaxPositive = 3 }, record);
			record.Add(4);
			Assert.Equivalent(new FakeIntRecord() { MinPositive = 2, MaxPositive = 4 }, record);
		}

		[Fact]
		public void Add_Negative_NegativeStats() {
			ReplayRecord record = Int(-3);
			Assert.Equivalent(new FakeIntRecord() { MinNegative = -3, MaxNegative = -3 }, record);
			record.Add(-2);
			Assert.Equivalent(new FakeIntRecord() { MinNegative = -3, MaxNegative = -2 }, record);
			record.Add(-4);
			Assert.Equivalent(new FakeIntRecord() { MinNegative = -4, MaxNegative = -2 }, record);
		}

		[Fact]
		public void Add_MinValue_NoStats() {
			Assert.Equivalent(new FakeIntRecord() { MinValue = true }, Int(int.MinValue));
		}

		[Fact]
		public void Add_MaxValue_NoStats() {
			Assert.Equivalent(new FakeIntRecord() { MaxValue = true }, Int(int.MaxValue));
		}

		private OrderedStringRecord String(string value) {
			OrderedStringRecord r = new OrderedStringRecord();
			r.Add(value);
			return r;
		}

		[Fact]
		public void Add_NullString_NullNoFrequency() {
			OrderedStringRecord record = String(null);
			Assert.Equal(1, record.NullCount);
			Assert.Empty(record.Frequency);
			Assert.Equivalent(new List<string>() { null }, record.Order);
		}

		[Fact]
		public void Add_NullString_NullRepeatNoFrequency() {
			OrderedStringRecord record = String(null);
			record.Add(null);
			Assert.Equal(2, record.NullCount);
			Assert.Empty(record.Frequency);
			Assert.Equivalent(new List<string>() { null }, record.Order);
		}

		[Fact]
		public void Add_String_Frequency() {
			OrderedStringRecord record = String("a");
			Assert.Equal(0, record.NullCount);
			Assert.Equivalent(new Dictionary<string, int>() { { "a", 1 } }, record.Frequency);
			Assert.Equivalent(new List<string>() { "a" }, record.Order);
		}

		[Fact]
		public void Add_StringRepeat_Frequency() {
			OrderedStringRecord record = String("a");
			record.Add("a");
			Assert.Equal(0, record.NullCount);
			Assert.Equivalent(new Dictionary<string, int>() { { "a", 2 } }, record.Frequency);
			Assert.Equivalent(new List<string>() { "a" }, record.Order);
		}

		[Fact]
		public void Add_Strings_Frequency() {
			OrderedStringRecord record = String("a");
			record.Add("b");
			Assert.Equal(0, record.NullCount);
			Assert.Equivalent(new Dictionary<string, int>() { { "a", 1 }, { "b", 1 } }, record.Frequency);
			Assert.Equivalent(new List<string>() { "a", "b" }, record.Order);
		}
	}
}
