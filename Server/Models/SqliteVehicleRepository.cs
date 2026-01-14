using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public class SqliteVehicleRepository {
		private readonly ILogger<SqliteVehicleRepository> logger;
		private readonly SqliteContext context;

		public SqliteVehicleRepository(ILogger<SqliteVehicleRepository> logger, SqliteContext context) {
			this.logger = logger;
			this.context = context;
		}

		public async Task Repair() {
			await RepairOfflineCars();
			await RepairOnlineCars();
			await CheckVehs();
		}

		private async Task RepairOfflineCars() {
			// all car info except custom team number
			string query = """
				UPDATE Cars
				SET
					VehicleName = case when Cars.VehicleName is null OR Cars.VehicleName == '' then v.Name else Cars.VehicleName end,
					TeamName = case when Cars.TeamName is null OR Cars.TeamName == '' then v.Team else Cars.TeamName end,
					Class = case when Cars.Class is null OR Cars.Class == '' then (case when v.Class == 'Hypercar' then 'Hyper' else v.Class end) else Cars.Class end,
					Number = case when (Cars.Number is null OR Cars.Number == '') AND v.Livery != "Custom" then v.Number else Cars.Number end
				FROM (
					SELECT c.CarId, v.Name, v.Team, v.Class, v.Number, v.Livery
					FROM Cars c
					INNER JOIN Vehicles v on v.Id == c.Veh
					INNER JOIN Sessions s on s.SessionId == c.SessionId
					WHERE c.Veh == v.Id AND NOT s.IsOnline
				) as v
				WHERE Cars.CarId == v.CarId
				""";
			await context.Database.ExecuteSqlRawAsync(query);
		}

		private async Task RepairOnlineCars() {
			// skip number and team name for custom teams
			string query = """
				UPDATE Cars
				SET
					VehicleName = case when Cars.VehicleName is null OR Cars.VehicleName == '' then v.Name else Cars.VehicleName end,
					TeamName = case when (Cars.TeamName is null OR Cars.TeamName == '') AND v.Livery != "Custom" then v.Team else Cars.TeamName end,
					Class = case when Cars.Class is null OR Cars.Class == '' then (case when v.Class == 'Hypercar' then 'Hyper' else v.Class end) else Cars.Class end,
					Number = case when (Cars.Number is null OR Cars.Number == '') AND v.Livery != "Custom" then v.Number else Cars.Number end
				FROM (
					SELECT c.CarId, v.Name, v.Team, v.Class, v.Number, v.Livery
					FROM Cars c
					INNER JOIN Vehicles v on v.Id == c.Veh
					INNER JOIN Sessions s on s.SessionId == c.SessionId
					WHERE c.Veh == v.Id AND s.IsOnline
				) as v
				WHERE Cars.CarId == v.CarId
				""";
			await context.Database.ExecuteSqlRawAsync(query);
		}

		public async Task CheckVehs() {
			List<string> vehs = await context.Cars
				.Where(x => x.Veh != null && x.Veh != "")
				.GroupBy(x => x.Veh)
				.Where(x => context.Vehicles.Where(y => y.Id == x.Key).Count() == 0)
				.Select(x => x.Key)
				.ToListAsync();
			if(vehs.Count > 0)
				logger.LogDebug($"Unknown vehs: {string.Join(", ", vehs)}");
		}
	}
}
