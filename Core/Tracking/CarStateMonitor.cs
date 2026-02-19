using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarStateMonitor {
		private readonly Dictionary<CarKey, CarState> states = new Dictionary<CarKey, CarState>();
		private readonly List<CarKey> keysToLogChanges = new List<CarKey>() {
			// add keys here to log car state changes for
			//new CarKey("56-397_24_MCLAREN"),
			//new CarKey("21-397_25ELMS_ORECA07"),
		};

		public CarStateMonitor(List<CarState> carStates = null) {
			if(carStates != null) {
				foreach(CarState carState in carStates) {
					if(carState != null)
						states.Add(carState.Key, carState);
				}
			}
		}

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
			if(!states.TryGetValue(key, out CarState oldState)) {
				states.Add(key, new CarState(key, standing));
				return null;
			}
			CarState newState = oldState.Next(standing);
			states[key] = newState;
			if(!keysToLogChanges.Contains(key))
				return null;
			List<string> changes = oldState.Difference(newState);
			if(changes.Count == 0)
				return null;
			else
				return $"Car {key.Id()} changed state: ({string.Join(", ", changes)})";
		}

		public CarState GetState(CarKey key) {
			if(states.TryGetValue(key, out CarState state))
				return state;
			return null;
		}

		public List<CarState> GetAllStates() {
			return new List<CarState>(states.Values);
		}
	}
}
