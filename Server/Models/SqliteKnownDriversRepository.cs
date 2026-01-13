using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Models {
	public class SqliteKnownDriversRepository {
		private readonly ILogger<SqliteKnownDriversRepository> logger;
		private readonly SqliteContext context;
		private readonly KnownDriversOptions options;

		public SqliteKnownDriversRepository(ILogger<SqliteKnownDriversRepository> logger, SqliteContext context, IOptions<KnownDriversOptions> options) {
			this.logger = logger;
			this.context = context;
			this.options = options.Value ?? new KnownDriversOptions();
			this.options.Add ??= new List<string>();
			this.options.Remove ??= new List<string>();
		}

		public async Task Apply() {
			foreach(string name in options.Add) {
				KnownDriver driver = await context.KnownDrivers.FirstOrDefaultAsync(x => x.Name == name);
				if(driver == null) {
					context.KnownDrivers.Add(new KnownDriver() { Name = name });
					logger.LogDebug($"Added known driver: {name}");
				}
			}
			foreach(string name in options.Remove) {
				KnownDriver driver = await context.KnownDrivers.FirstOrDefaultAsync(x => x.Name == name);
				if(driver != null) {
					context.KnownDrivers.Remove(driver);
					logger.LogDebug($"Removed known driver: {name}");
				}
			}
			await context.SaveChangesAsync();
		}
	}
}
