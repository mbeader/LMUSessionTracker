using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session.Replay {
	public class CarReplay {
		public CarKey Key { get; set; }
		public List<LapReplay> Laps = new List<LapReplay>();

		public void Update(Standing standing) {
			if(Laps.Count == 0 || Laps[^1].Num != standing.lapsCompleted)
				Laps.Add(new LapReplay() { Num = standing.lapsCompleted });
			Laps[^1].Update(standing);
		}
	}
}
