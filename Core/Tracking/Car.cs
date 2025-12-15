using LMUSessionTracker.Core.LMU;

namespace LMUSessionTracker.Core.Tracking {
	public class Car {
		public int SlotId { get; set; }
		public string Veh { get; set; }
		public string VehicleName { get; set; }
		public string TeamName { get; set; }
		public string Class { get; set; }
		public string Number { get; set; }
		public string Id { get; set; }

		public Car() { }

		public Car(Standing standing) {
			SlotId = standing.slotID;
			Veh = standing.vehicleFilename;
			VehicleName = standing.vehicleName;
			TeamName = standing.fullTeamName;
			Class = standing.carClass;
			Number = standing.carNumber;
			Id = standing.carId;
		}

		public void Merge(Car from) {
			VehicleName = Merge(VehicleName, from.VehicleName);
			TeamName = Merge(TeamName, from.TeamName);
			Class = Merge(Class, from.Class);
			Number = Merge(Number, from.Number);
			Id = Merge(Id, from.Id);
		}

		private string Merge(string a, string b) {
			return string.IsNullOrEmpty(a) ? b : a;
		}
	}
}
