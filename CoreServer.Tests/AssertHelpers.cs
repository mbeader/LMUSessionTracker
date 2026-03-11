using System;
using System.Collections.Generic;

namespace LMUSessionTracker.CoreServer.Tests {
	public static class AssertHelpers {
		public static void Equivalent<T>(List<T> ex, List<T> ac) {
			Action<T>[] actions = new Action<T>[ex.Count];
			for(int i = 0; i < ex.Count; i++) {
				T item = ex[i];
				actions[i] = ac => Assert.Equivalent(item, ac);
			}
			Assert.Collection(ac, actions);
		}
	}
}
