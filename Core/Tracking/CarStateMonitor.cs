using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarStateMonitor {
		private readonly Dictionary<CarKey, CarState> states = new Dictionary<CarKey, CarState>();

		public List<string> Update(List<Standing> standings) {
			List<string> changes = new List<string>();
			foreach(Standing standing in standings) {
				string change = Update(standing);
				if(change != null)
					changes.Add(change);
			}
			return changes;
		}

		private string Update(Standing standing) {
			CarKey key = new CarKey(standing.slotID, standing.vehicleFilename);
			CarState newState = CarState.From(standing);
			if(!states.TryGetValue(key, out CarState oldState)) {
				states.Add(key, newState);
				return null;
			} else
				states[key] = newState;
			List<string> changes = oldState.Difference(newState);
			if(changes.Count == 0)
				return null;
			else
				return $"Car {key.Id()} changed state: ({string.Join(", ", changes)})";
		}
	}
}
