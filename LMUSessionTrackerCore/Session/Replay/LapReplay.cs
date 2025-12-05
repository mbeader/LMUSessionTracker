using LMUSessionTracker.Core.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Session.Replay {
	public class LapReplay {
		public int Num { get; init; }
		//public List<AttackMode> attackMode { get; set; }
		//public List<Velocity> carAcceleration { get; set; }
		//public List<Position> carPosition { get; set; }
		//public List<Velocity> carVelocity { get; set; }
		public List<double> bestLapSectorTime1 { get; } = new List<double>();
		public List<double> bestLapSectorTime2 { get; } = new List<double>();
		public List<double> bestLapTime { get; } = new List<double>();
		public List<double> bestSectorTime1 { get; } = new List<double>();
		public List<double> bestSectorTime2 { get; } = new List<double>();
		public List<string> carClass { get; } = new List<string>();
		public List<string> carId { get; } = new List<string>();
		public List<string> carNumber { get; } = new List<string>();
		public List<string> countLapFlag { get; } = new List<string>();
		public List<double> currentSectorTime1 { get; } = new List<double>();
		public List<double> currentSectorTime2 { get; } = new List<double>();
		public List<string> driverName { get; } = new List<string>();
		public List<bool> drsActive { get; } = new List<bool>();
		public List<double> estimatedLapTime { get; } = new List<double>();
		public List<string> finishStatus { get; } = new List<string>();
		public List<string> flag { get; } = new List<string>();
		public List<bool> focus { get; } = new List<bool>();
		public List<double> fuelFraction { get; } = new List<double>();
		public List<string> fullTeamName { get; } = new List<string>();
		public List<string> gamePhase { get; } = new List<string>();
		public List<bool> hasFocus { get; } = new List<bool>();
		public List<bool> headlights { get; } = new List<bool>();
		public List<int> inControl { get; } = new List<int>();
		public List<bool> inGarageStall { get; } = new List<bool>();
		public List<double> lapDistance { get; } = new List<double>();
		public List<double> lapStartET { get; } = new List<double>();
		public List<int> lapsBehindLeader { get; } = new List<int>();
		public List<int> lapsBehindNext { get; } = new List<int>();
		public List<int> lapsCompleted { get; } = new List<int>();
		public List<double> lastLapTime { get; } = new List<double>();
		public List<double> lastSectorTime1 { get; } = new List<double>();
		public List<double> lastSectorTime2 { get; } = new List<double>();
		public List<double> pathLateral { get; } = new List<double>();
		public List<int> penalties { get; } = new List<int>();
		public List<string> pitGroup { get; } = new List<string>();
		public List<double> pitLapDistance { get; } = new List<double>();
		public List<string> pitState { get; } = new List<string>();
		public List<int> pitstops { get; } = new List<int>();
		public List<bool> pitting { get; } = new List<bool>();
		public List<bool> player { get; } = new List<bool>();
		public List<int> position { get; } = new List<int>();
		public List<int> qualification { get; } = new List<int>();
		public List<string> sector { get; } = new List<string>();
		public List<bool> serverScored { get; } = new List<bool>();
		public List<int> slotID { get; } = new List<int>();
		public List<long> steamID { get; } = new List<long>();
		public List<double> timeBehindLeader { get; } = new List<double>();
		public List<double> timeBehindNext { get; } = new List<double>();
		public List<double> timeIntoLap { get; } = new List<double>();
		public List<double> trackEdge { get; } = new List<double>();
		public List<bool> underYellow { get; } = new List<bool>();
		public List<string> upgradePack { get; } = new List<string>();
		public List<string> vehicleFilename { get; } = new List<string>();
		public List<string> vehicleName { get; } = new List<string>();

		public void Update(Standing s) {
			AddIfChanged(bestLapSectorTime1, s.bestLapSectorTime1);
			AddIfChanged(bestLapSectorTime2, s.bestLapSectorTime2);
			AddIfChanged(bestLapTime, s.bestLapTime);
			AddIfChanged(bestSectorTime1, s.bestSectorTime1);
			AddIfChanged(bestSectorTime2, s.bestSectorTime2);
			AddIfChanged(carClass, s.carClass);
			AddIfChanged(carId, s.carId);
			AddIfChanged(carNumber, s.carNumber);
			AddIfChanged(countLapFlag, s.countLapFlag);
			AddIfChanged(currentSectorTime1, s.currentSectorTime1);
			AddIfChanged(currentSectorTime2, s.currentSectorTime2);
			AddIfChanged(driverName, s.driverName);
			AddIfChanged(drsActive, s.drsActive);
			AddIfChanged(estimatedLapTime, s.estimatedLapTime);
			AddIfChanged(finishStatus, s.finishStatus);
			AddIfChanged(flag, s.flag);
			AddIfChanged(focus, s.focus);
			AddIfChanged(fuelFraction, s.fuelFraction);
			AddIfChanged(fullTeamName, s.fullTeamName);
			AddIfChanged(gamePhase, s.gamePhase);
			AddIfChanged(hasFocus, s.hasFocus);
			AddIfChanged(headlights, s.headlights);
			AddIfChanged(inControl, s.inControl);
			AddIfChanged(inGarageStall, s.inGarageStall);
			AddIfChanged(lapDistance, s.lapDistance);
			AddIfChanged(lapStartET, s.lapStartET);
			AddIfChanged(lapsBehindLeader, s.lapsBehindLeader);
			AddIfChanged(lapsBehindNext, s.lapsBehindNext);
			AddIfChanged(lapsCompleted, s.lapsCompleted);
			AddIfChanged(lastLapTime, s.lastLapTime);
			AddIfChanged(lastSectorTime1, s.lastSectorTime1);
			AddIfChanged(lastSectorTime2, s.lastSectorTime2);
			AddIfChanged(pathLateral, s.pathLateral);
			AddIfChanged(penalties, s.penalties);
			AddIfChanged(pitGroup, s.pitGroup);
			AddIfChanged(pitLapDistance, s.pitLapDistance);
			AddIfChanged(pitState, s.pitState);
			AddIfChanged(pitstops, s.pitstops);
			AddIfChanged(pitting, s.pitting);
			AddIfChanged(player, s.player);
			AddIfChanged(position, s.position);
			AddIfChanged(qualification, s.qualification);
			AddIfChanged(sector, s.sector);
			AddIfChanged(serverScored, s.serverScored);
			AddIfChanged(slotID, s.slotID);
			AddIfChanged(steamID, s.steamID);
			AddIfChanged(timeBehindLeader, s.timeBehindLeader);
			AddIfChanged(timeBehindNext, s.timeBehindNext);
			AddIfChanged(timeIntoLap, s.timeIntoLap);
			AddIfChanged(trackEdge, s.trackEdge);
			AddIfChanged(underYellow, s.underYellow);
			AddIfChanged(upgradePack, s.upgradePack);
			AddIfChanged(vehicleFilename, s.vehicleFilename);
			AddIfChanged(vehicleName, s.vehicleName);
		}

		private void AddIfChanged<T>(List<T> l, T v) {
			if(l.Count == 0 || !l[^1].Equals(v))
				l.Add(v);
		}
	}
}
