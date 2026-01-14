using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMUSessionTracker.Server.Models {
	public class VehicleModel {
		[Key, Required]
		public string Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Engine { get; set; }
		[Required]
		public string Manufacturer { get; set; }

		public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

		public VehicleModel() { }

		public VehicleModel(string id, string name, string engine, string manufacturer) {
			Id = id;
			Name = name;
			Engine = engine;
			Manufacturer = manufacturer;
		}
	}
}
