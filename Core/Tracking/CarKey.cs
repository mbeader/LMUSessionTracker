using System;

namespace LMUSessionTracker.Core.Tracking {
	public readonly struct CarKey {
		public int SlotId { get; init; }
		public string Veh { get; init; }

		public CarKey() { }

		public CarKey(int slotId, string veh) {
			SlotId = slotId;
			Veh = veh;
		}

		public CarKey(string id) {
			int dash = id.IndexOf('-');
			if(dash < 0)
				throw new ArgumentException(nameof(id));
			SlotId = int.Parse(id[..dash]);
			Veh = id[(dash + 1)..];
		}

		public bool Matches(string s) {
			return s == Id();
		}

		public string Id() => $"{SlotId}-{Veh}";

		public override string ToString() => Id();
	}
}
