namespace LMUSessionTracker.Core.LMU {
	public class StandingsHistoryLap {
		public string carClass { get; set; }
		public string driverName { get; set; }
		public string finishStatus { get; set; }
		public double lapTime { get; set; }
		public bool pitting { get; set; }
		public int position { get; set; }
		public double sectorTime1 { get; set; }
		public double sectorTime2 { get; set; }
		public int slotID { get; set; }
		public int totalLaps { get; set; }
		public string vehicleName { get; set; }
	}
}
