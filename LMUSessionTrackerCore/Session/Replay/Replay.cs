using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session.Replay {
	public class Replay {
		public Dictionary<CarKey, CarReplay> Cars { get; } = new Dictionary<CarKey, CarReplay>();

		public void Update(List<Standing> standings) {
			foreach(Standing standing in standings) {
				CarKey key = new CarKey() { SlotId = standing.slotID, Veh = standing.vehicleFilename };
				if(!Cars.TryGetValue(key, out CarReplay replay)) {
					replay = new CarReplay() { Key = key };
					Cars.Add(key, replay);
				}
				replay.Update(standing);
			}
		}
	}
}
