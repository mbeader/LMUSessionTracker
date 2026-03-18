using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Tests {
	public static class AssertHelpers {
		public static void Equivalent<T>(List<T> ex, List<T> ac) {
			Action<T>[] actions = new Action<T>[ex.Count];
			for(int i = 0; i < ex.Count; i++) {
				T item = ex[i];
				actions[i] = ac => Assert.Equivalent(item, ac);
			}
			Assert.Collection(ac, actions);
		}

		public static void Equivalent<TKey, TValue>(Dictionary<TKey, List<TValue>> ex, Dictionary<TKey, List<TValue>> ac) {
			Action<KeyValuePair<TKey, List<TValue>>>[] actions = new Action<KeyValuePair<TKey, List<TValue>>>[ex.Count];
			List<KeyValuePair<TKey, List<TValue>>> acList = new List<KeyValuePair<TKey, List<TValue>>>(ac);
			Assert.Equal(new List<TKey>(ex.Keys), acList.ConvertAll(x => x.Key));
			for(int i = 0; i < acList.Count; i++) {
				List<TValue> item = ex[acList[i].Key];
				actions[i] = ac => { Equivalent(item, ac.Value); };
			}
			Assert.Collection(acList, actions);
		}
	}
}
