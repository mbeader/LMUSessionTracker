using LMUSessionTracker.Core.Tracking;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Services {
	public interface VehicleService {
		public void Init(List<Vehicle> vehicles);
		public Vehicle GetVehicle(string veh);
	}

	public class DefaultVehicleService : VehicleService {
		private readonly ConcurrentDictionary<string, Vehicle> vehs = new ConcurrentDictionary<string, Vehicle>();
		private readonly ILogger<DefaultVehicleService> logger;

		public DefaultVehicleService(ILogger<DefaultVehicleService> logger) {
			this.logger = logger;
		}

		public void Init(List<Vehicle> vehicles) {
			foreach(Vehicle vehicle in vehicles) {
				vehs.AddOrUpdate(vehicle.Id, vehicle, (_, _) => vehicle);
			}
		}

		public Vehicle GetVehicle(string veh) {
			if(vehs.TryGetValue(veh, out Vehicle vehicle))
				return vehicle;
			return null;
		}
	}
}
