using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class CarStateMonitor {
		private readonly Dictionary<CarKey, CarStateChange> states = new Dictionary<CarKey, CarStateChange>();
		private readonly List<CarKey> keysToLogChanges = new List<CarKey>() {
			// add keys here to log car state changes for
			//new CarKey("56-397_24_MCLAREN"),
			//new CarKey("21-397_25ELMS_ORECA07"),
		};

		public CarStateMonitor(List<CarState> carStates = null) {
			if(carStates != null) {
				foreach(CarState carState in carStates) {
					if(carState != null)
						states.Add(carState.Key, new CarStateChange(carState));
				}
			}
		}

		public List<string> Update(double currentET, List<Standing> standings) {
			List<string> changes = new List<string>();
			foreach(Standing standing in standings) {
				string change = Update(currentET, standing);
				if(change != null)
					changes.Add(change);
			}
			return changes;
		}

		private string Update(double currentET, Standing standing) {
			CarKey key = new CarKey(standing.slotID, standing.vehicleFilename);
			if(!states.TryGetValue(key, out CarStateChange state)) {
				states.Add(key, new CarStateChange(new CarState(key, standing)));
				return null;
			}
			CarState newState = state.Current.Next(currentET, standing);
			state.Next(newState);
			if(!keysToLogChanges.Contains(key))
				return null;
			List<string> changes = state.Previous.Difference(newState);
			if(changes.Count == 0)
				return null;
			else
				return $"Car {key.Id()} changed state: ({string.Join(", ", changes)})";
		}

		public CarStateChange GetState(CarKey key) {
			if(states.TryGetValue(key, out CarStateChange state))
				return state;
			return null;
		}

		public CarState GetPreviousState(CarKey key) {
			return GetState(key)?.Previous;
		}

		public CarState GetCurrentState(CarKey key) {
			return GetState(key)?.Current;
		}

		public List<CarState> GetAllStates() {
			List<CarState> res = new List<CarState>(states.Count);
			foreach(CarStateChange state in states.Values)
				res.Add(state.Current);
			return res;
		}
	}
}
