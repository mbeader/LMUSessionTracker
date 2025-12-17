using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Car {
		[Key, Required]
		public long CarId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }
		[ForeignKey(nameof(Entry))]
		public long? EntryId { get; set; }

		[Required]
		public int SlotId { get; set; }
		[Required]
		public string Veh { get; set; }
		public string VehicleName { get; set; }
		public string TeamName { get; set; }
		public string Class { get; set; }
		public string Number { get; set; }
		public string Id { get; set; }

		public Entry Entry { get; set; }
		public Session Session { get; set; }
		public ICollection<Lap> Laps { get; } = new List<Lap>();

		public void From(Core.Tracking.Car car) {
			if(Veh != null && (SlotId != car.SlotId || Veh != car.Veh))
				throw new InvalidOperationException($"Cannot change lap from {SlotId}-{Veh} to {car.SlotId}-{car.Veh}");
			SlotId = car.SlotId;
			Veh = car.Veh;
			VehicleName = car.VehicleName;
			TeamName = car.TeamName;
			Class = car.Class;
			Number = car.Number;
			Id = car.Id;
		}

		public Core.Tracking.Car To() {
			return new Core.Tracking.Car() {
				SlotId = SlotId,
				Veh = Veh,
				VehicleName = VehicleName,
				TeamName = TeamName,
				Class = Class,
				Number = Number,
				Id = Id,
			};
		}
	}
}
