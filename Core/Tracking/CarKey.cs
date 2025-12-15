namespace LMUSessionTracker.Core.Tracking {
	public readonly struct CarKey {
		public int SlotId { get; init; }
		public string Veh { get; init; }

		public bool Matches(string s) {
			return s == Id();
		}

		public string Id() => $"{SlotId}-{Veh}";
	}
}
