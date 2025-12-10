using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Replay {
	public class ReplaySession {
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
