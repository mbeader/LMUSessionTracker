using System.Collections.Generic;
using System.Numerics;

namespace LMUSessionTracker.Core.Replay {
	public interface ReplayRecord {
		public void Add(object value);
	}

	public interface ReplayRecord<T> : ReplayRecord {
		public void Add(T value);
	}

	public abstract class EnumerableRecord<T> : ReplayRecord<T> {
		public int NullCount { get; private set; }
		public Dictionary<T, int> Frequency { get; } = new Dictionary<T, int>();

		public virtual void Add(T value) {
			if(value == null)
				NullCount++;
			else if(Frequency.ContainsKey(value))
				Frequency[value] += 1;
			else
				Frequency.Add(value, 1);
		}

		public void Add(object value) => Add((T)value);
	}

	public abstract class OrderedEnumerableRecord<T> : EnumerableRecord<T> {
		public List<OrderedItem<T>> Order { get; } = new List<OrderedItem<T>>();

		public override void Add(T value) {
			base.Add(value);
			if(Order.Count == 0 || (Order[^1].Value == null && value != null) || (Order[^1].Value != null && !Order[^1].Value.Equals(value)))
				Order.Add(new OrderedItem<T>(value));
			Order[^1].Count++;
		}
	}

	public class OrderedItem<T> {
		public T Value { get; set; }
		public int Count { get; set; }

		public OrderedItem() { }

		public OrderedItem(T value, int count = 0) {
			Value = value;
			Count = count;
		}
	}

	public class StringRecord : EnumerableRecord<string> {
	}

	public class OrderedStringRecord : OrderedEnumerableRecord<string> {
	}

	public abstract class DiscreteRecord<T> : OrderedEnumerableRecord<T?> where T : struct, IBinaryInteger<T> {
	}

	public abstract class NumericRecord<T> : ReplayRecord<T?> where T : struct, INumber<T> {
		public bool Null { get; protected set; }
		public bool MaxValue { get; protected set; }
		public bool MinValue { get; protected set; }
		public bool Zero { get; protected set; }
		public bool NegativeOne { get; protected set; }
		public T? MaxPositive { get; protected set; }
		public T? MinPositive { get; protected set; }
		public T? MaxNegative { get; protected set; }
		public T? MinNegative { get; protected set; }

		public abstract void Add(T? value);

		public void Add(object value) => Add((T?)value);

		protected void Add(T? nullableValue, T maxValue, T minValue, T zero, T? negativeOne) {
			if(nullableValue.HasValue) {
				T value = nullableValue.Value;
				if(value == zero) {
					Zero = true;
				} else if(value > zero) {
					if(value == maxValue) {
						MaxValue = true;
					} else if(value == minValue) {
						MinValue = true;
					} else if(!MinPositive.HasValue) {
						MinPositive = value;
						MaxPositive = value;
					} else if(value < MinPositive) {
						MinPositive = value;
					} else if(value > MaxPositive) {
						MaxPositive = value;
					}
				} else {
					if(value == negativeOne.Value)
						NegativeOne = true;
					if(value == maxValue) {
						MaxValue = true;
					} else if(value == minValue) {
						MinValue = true;
					} else if(!MaxNegative.HasValue) {
						MaxNegative = value;
						MinNegative = value;
					} else if(value < MinNegative) {
						MinNegative = value;
					} else if(value > MaxNegative) {
						MaxNegative = value;
					}
				}
			} else
				Null = true;
		}
	}

	public class IntRecord : NumericRecord<int> {
		public override void Add(int? value) {
			Add(value, int.MaxValue, int.MinValue, 0, -1);
		}
	}

	public class DiscreteIntRecord : DiscreteRecord<int> {
	}

	public class UIntRecord : NumericRecord<uint> {
		public override void Add(uint? value) {
			Add(value, uint.MaxValue, uint.MinValue, 0, null);
		}
	}

	public class DiscreteUIntRecord : DiscreteRecord<uint> {
	}

	public class LongRecord : NumericRecord<long> {
		public override void Add(long? value) {
			Add(value, long.MaxValue, long.MinValue, 0L, -1L);
		}
	}

	public class DoubleRecord : NumericRecord<double> {
		public override void Add(double? value) {
			Add(value, double.PositiveInfinity, double.NegativeInfinity, 0.0, -1.0);
		}
	}

	public class BoolRecord : EnumerableRecord<bool?> {
	}

	public class OrderedBoolRecord : OrderedEnumerableRecord<bool?> {
	}
}
