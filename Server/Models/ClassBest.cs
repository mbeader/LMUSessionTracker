namespace LMUSessionTracker.Server.Models {
	/// <summary>
	/// Values of -3 indicate invalid sector (to distinguish from actual invalid sector -1)
	/// </summary>
	public class ClassBest {
		public double TotalTime { get; set; }
		public double Sector1 { get; set; }
		public double Sector2 { get; set; }
		public double Sector3 { get; set; }
	}
}
