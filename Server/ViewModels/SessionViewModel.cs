using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.CoreServer.Tracking;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class SessionViewModel {
		public SessionInfo Info { get; set; }
		public List<Standing> Standings { get; set; }
		public List<CarHistory> History { get; set; }
		public List<CarState> CarState { get; set; }
		public Dictionary<CarKey, int> PositionInClass { get; } = new Dictionary<CarKey, int>();
		public Dictionary<CarKey, Car> Entries { get; } = new Dictionary<CarKey, Car>();
		public List<Result> Results { get; set; }
		public Bests Bests { get; set; }

		public Models.Session Session { get; set; }
		public Models.SessionState SessionState => Session?.LastState;
		public Models.Session NextSession { get; set; }

		public void SetSession(Session session, bool includeBests = true) {
			if(session != null) {
				Standings = session.LastStandings;
				History = session.History.GetAllHistory();
				CarState = session.CarState.GetAllStates();
				if(Standings != null)
					SetPIC(Standings, x => x.carClass, x => new CarKey() { SlotId = x.slotID, Veh = x.vehicleFilename });
				History.ForEach(x => Entries.Add(x.Key, x.Car));
				if(includeBests)
					Bests = session.Bests;
			} else if(Session != null) {
				Session coreSession = Session.To();
				History = coreSession.History.GetAllHistory();
				if(Results != null)
					SetPIC(Results, x => x.Car.Class, x => new CarKey() { SlotId = x.Car.SlotId, Veh = x.Car.Veh });
				History.ForEach(x => Entries.Add(x.Key, x.Car));
				Bests = coreSession.Bests;
			}
		}

		private void SetPIC<T>(List<T> list, Func<T, string> carClass, Func<T, CarKey> key) {
			Dictionary<string, List<CarKey>> classes = new Dictionary<string, List<CarKey>>();
			foreach(T item in list) {
				string classId = carClass(item) ?? string.Empty;
				if(!classes.TryGetValue(classId, out List<CarKey> cars)) {
					cars = new List<CarKey>();
					classes.Add(classId, cars);
				}
				cars.Add(key(item));
			}
			foreach(string classname in classes.Keys)
				for(int i = 0; i < classes[classname].Count; i++)
					PositionInClass.Add(classes[classname][i], i + 1);
		}
	}
}
