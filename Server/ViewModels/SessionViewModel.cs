using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.ViewModels {
	public class SessionViewModel {
		public SessionInfo Info { get; set; }
		public List<Standing> Standings { get; set; }
		public List<CarHistory> History { get; set; }
		public List<CarState> CarState { get; set; }
		public Dictionary<CarKey, int> PositionInClass { get; } = new Dictionary<CarKey, int>();
		public Dictionary<CarKey, Car> Entries { get; } = new Dictionary<CarKey, Car>();
		public List<Models.Lap> Results { get; set; }
		public Bests Bests { get; set; }

		public Models.Session Session { get; set; }
		public Models.SessionState SessionState => Session?.LastState;
		public Models.Session NextSession { get; set; }

		public void SetSession(Session session) {
			if(session != null) {
				Standings = session.LastStandings;
				History = session.History.GetAllHistory();
				CarState = session.CarState.GetAllStates();
				Dictionary<string, List<CarKey>> classes = new Dictionary<string, List<CarKey>>();
				if(Standings != null) {
					foreach(Standing standings in Standings) {
						if(!classes.TryGetValue(standings.carClass, out List<CarKey> cars)) {
							cars = new List<CarKey>();
							classes.Add(standings.carClass, cars);
						}
						cars.Add(new CarKey() { SlotId = standings.slotID, Veh = standings.vehicleFilename });
					}
				}
				foreach(string classname in classes.Keys)
					for(int i = 0; i < classes[classname].Count; i++)
						PositionInClass.Add(classes[classname][i], i + 1);
				History.ForEach(x => Entries.Add(x.Key, x.Car));
			} else if(Session != null) {
				Session coreSession = Session.To();
				History = coreSession.History.GetAllHistory();
				History.ForEach(x => Entries.Add(x.Key, x.Car));
			}
		}
	}
}
