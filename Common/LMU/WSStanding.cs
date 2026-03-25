using System.Collections.Generic;

namespace LMUSessionTracker.Common.LMU {
	public class WSStanding {
		public double bestIndividualTimeSector1 { get; set; }
		public double bestIndividualTimeSector2 { get; set; }
		public double bestLapTime { get; set; }
		public double bestLapTimeS1 { get; set; }
		public double bestLapTimeS2 { get; set; }
		public List<string> compoundNames { get; set; }
		public string countLapFlag { get; set; }
		public double currentLapTimeS1 { get; set; }
		public double currentLapTimeS2 { get; set; }
		public int currentLapsCompleted { get; set; }
		public int currentSector { get; set; }
		public int dlcAppID { get; set; }
		public string driverName { get; set; }
		public string finishStatus { get; set; }
		public string gapToClassLeader { get; set; }
		public string gapToRelativeTxt { get; set; }
		public bool inGarage { get; set; }
		public bool inPits { get; set; }
		public double lapDist { get; set; }
		public int lapsBehindLeader { get; set; }
		public int lapsBehindNext { get; set; }
		public double lastLapTime { get; set; }
		public double lastLapTimeS1 { get; set; }
		public double lastLapTimeS2 { get; set; }
		public double localVelX { get; set; }
		public double localVelY { get; set; }
		public double localVelZ { get; set; }
		public int numPitstops { get; set; }
		public WSPenalties penalties { get; set; }
		public int pitState { get; set; }
		public bool player { get; set; }
		public double posX { get; set; }
		public double posY { get; set; }
		public double posZ { get; set; }
		public int position { get; set; }
		public int qualiPosition { get; set; }
		public bool slotHasFocus { get; set; }
		public int slotID { get; set; }
		public string steamID { get; set; }
		public string teamName { get; set; }
		public double timeBehindLeader { get; set; }
		public double timeBehindNext { get; set; }
		public double timeIntoLap { get; set; }
		public string tireCompoundNameFront { get; set; }
		public string tireCompoundNameRear { get; set; }
		public double totalRaceTime { get; set; }
		public string vehFilename { get; set; }
		public bool vehOwned { get; set; }
		public string vehicleClass { get; set; }
		public string vehicleName { get; set; }
		public string vehicleNumber { get; set; }
		public double virtualEnergy { get; set; }
	}

	/// <summary>
	/// Does not correspond to a model returned by the LMU API
	/// </summary>
	public class WSStandingSubset {
		public List<string> compoundNames { get; set; }
		public WSPenalties penalties { get; set; }
		public int slotID { get; set; }
		public string vehFilename { get; set; }
		public double virtualEnergy { get; set; }
	}
}
