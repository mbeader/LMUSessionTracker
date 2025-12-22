using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace LMUSessionTracker.Core.Replay {
	public class ReplayCollection {
		private readonly Dictionary<string, ReplayRecord> mapping = new Dictionary<string, ReplayRecord>() {
			{ "SessionInfo.playerName", new StringRecord() },
		};

		public SortedDictionary<string, ReplayRecord> Data { get; } = new SortedDictionary<string, ReplayRecord>(NaturalSortStringComparer.InvariantCultureIgnoreCase);
		public HashSet<string> UnsupportedTypes { get; } = new HashSet<string>();

		public void Add(ReplayRun run) {
			AddProperties(run, "", false);
		}

		private void AddProperties(object obj, string prefix, bool simple) {
			if(obj == null)
				return;
			Type type = obj.GetType();
			foreach(PropertyInfo property in type.GetProperties()) {
				string name = $"{prefix}{property.Name}";
				object value = property.GetValue(obj);
				AddProperty(name, value, simple, property.PropertyType);
			}
		}

		private void AddProperty(string name, object value, bool simple, Type type) {
			if(type.GenericTypeArguments.Length > 0)
				AddGenericProperty(name, value, simple, type);
			else
				AddConcreteProperty(name, value, simple, type);
		}

		private void AddGenericProperty(string name, object value, bool simple, Type type) {
			if(type.Name == "List`1") {
				AddList(name, value);
			} else if(type.Name == "Nullable`1") {
				AddPrimitive(name, value, simple);
			} else if(type.Name == "Dictionary`2") {
				AddDictionary(name, value);
			} else {
			}
		}

		private void AddList(string name, object value) {
			name += "[]";
#pragma warning disable CS0168 // Variable is declared but never used
			try {
				if(value != null) {
					foreach(object subvalue in (ICollection)value) {
						AddConcreteProperty(name, subvalue, true, value.GetType().GenericTypeArguments[0]);
					}
				}
			} catch(Exception e) {
			}
#pragma warning restore CS0168 // Variable is declared but never used
		}

		private void AddDictionary(string name, object value, Type baseType = null) {
#pragma warning disable CS0168 // Variable is declared but never used
			try {
				if(value != null) {
					IDictionary dictionary = (IDictionary)value;
					foreach(object key in dictionary.Keys) {
						AddPrimitive($"{name}.Key", key, true);
						AddProperty($"{name}.Value", dictionary[key], true, (baseType ?? dictionary.GetType()).GenericTypeArguments[1]);
					}
				}
			} catch(Exception e) {
			}
#pragma warning restore CS0168 // Variable is declared but never used
		}

		private void AddConcreteProperty(string name, object value, bool simple, Type type) {
			if(type.Namespace == "LMUSessionTracker.Core.LMU") {
				if(type.Name == "StandingsHistory" || type.Name == "StrategyUsage")
					AddDictionary(name, value, type.BaseType);
				else
					AddProperties(value, $"{name}.", simple);
			} else {
				AddPrimitive(name, value, simple);
			}
		}

		private void AddPrimitive(string name, object value, bool simple) {
			if(!Data.TryGetValue(name, out ReplayRecord record)) {
				record = SelectRecord(name, value, simple);
				Data.Add(name, record);
			}
			record?.Add(value);
		}

		private ReplayRecord SelectRecord(string name, object value, bool simple) {
			if(mapping.TryGetValue(name, out ReplayRecord record))
				return record;
			switch(value) {
				case string:
					return simple ? new StringRecord() : new OrderedStringRecord();
				case int:
					return simple ? new IntRecord() : new DiscreteIntRecord();
				case uint:
					return simple ? new UIntRecord() : new DiscreteUIntRecord();
				case long:
					return new LongRecord();
				case double:
					return new DoubleRecord();
				case bool:
					return simple ? new BoolRecord() : new OrderedBoolRecord();
				default:
					if(value != null)
						UnsupportedTypes.Add(value.GetType().FullName);
					return null;
			}
		}

		public SortedDictionary<string, object> Build() {
			SortedDictionary<string, object> root = new SortedDictionary<string, object>(NaturalSortStringComparer.InvariantCultureIgnoreCase);
			List<string> path = new List<string>();
			foreach(string key in Data.Keys) {
				path.Clear();
				object value = Data[key];
				SortedDictionary<string, object> curr = root;
				string[] parts = key.Split('.');
				for(int i = 0; i < parts.Length - 1; i++) {
					if(!curr.TryGetValue(parts[i], out object next)) {
						next = new SortedDictionary<string, object>();
						curr.Add(parts[i], next);
					}
					curr = (SortedDictionary<string, object>)next;
				}
				curr.Add(parts[^1], value);
			}
			return root;
		}
	}
}
