using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMUSessionTracker.Server.Models {
	public class Vehicle {
		private static readonly List<string> seriesNames = new List<string>() { "WEC2023", "WEC2024", "WEC2025", "ELMS2025" };
		private static readonly List<string> classNames = new List<string>() { "Hypercar", "LMP2_ELMS", "LMP2", "LMP3", "GTE", "GT3" };

		[Key, Required]
		public string Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Number { get; set; }
		[Required]
		public string Team { get; set; }
		[Required]
		public string Livery { get; set; }
		[Required]
		public string Model { get; set; }
		[Required]
		public string Class { get; set; }
		[Required]
		public string Series { get; set; }
		public bool Custom { get; set; }
		
		[ForeignKey(nameof(Model))]
		public VehicleModel VehicleModel { get; set; }
		public ICollection<VehicleDriver> VehicleDrivers { get; set; } = new List<VehicleDriver>();

		public Vehicle() { }

		public Vehicle(string id, string name, string number, string team, string livery, string model, string className, string series) {
			Id = id;
			Name = name;
			Number = number;
			Team = team;
			Livery = livery;
			if(!seriesNames.Contains(series)) {
				if(seriesNames.Contains(model))
					(model, series) = (series, model);
				else if(seriesNames.Contains(className))
					(className, series) = (series, className);
				else
					throw new Exception($"No series found for veh {id}: [{model}, {className}, {series}]");
			}
			if(!classNames.Contains(className)) {
				if(classNames.Contains(model))
					(model, className) = (className, model);
				else if(classNames.Contains(series))
					(series, className) = (className, series);
				else
					throw new Exception($"No class found for veh {id}: [{model}, {className}, {series}]");
			}
			Model = model;
			Class = className;
			Series = series;
			Custom = livery == "Custom";
		}
	}
}
