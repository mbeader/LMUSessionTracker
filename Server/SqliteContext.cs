using LMUSessionTracker.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LMUSessionTracker.Server {
	public class SqliteContext : DbContext {
		public SqliteContext(DbContextOptions options) : base(options) {
		}

		public DbSet<Session> Sessions { get; set; }
		public DbSet<SessionState> SessionStates { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Lap> Laps { get; set; }
		public DbSet<Entry> Entries { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<KnownDriver> KnownDrivers { get; set; }
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<VehicleModel> VehicleModels { get; set; }
		public DbSet<VehicleDriver> VehicleDrivers { get; set; }
		public DbSet<SessionTransition> SessionTransitions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Entry>()
				.HasAlternateKey(x => new { x.SessionId, x.EntryId });
			modelBuilder.Entity<Car>()
				.HasAlternateKey(x => new { x.SessionId, x.CarId });
			modelBuilder.Entity<KnownDriver>()
				.HasAlternateKey(x => x.Name);
			modelBuilder.Entity<SessionTransition>()
				.HasAlternateKey(x => new { x.FromSessionId, x.ToSessionId });

			modelBuilder.Entity<Member>()
				.HasOne(x => x.Entry)
				.WithMany(x => x.Members)
				.HasForeignKey(x => new { x.SessionId, x.EntryId })
				.HasPrincipalKey(x => new { x.SessionId, x.EntryId });
			modelBuilder.Entity<Car>()
				.HasOne(x => x.Entry)
				.WithOne(x => x.Car)
				.IsRequired(false)
				.HasForeignKey<Car>(x => new { x.SessionId, x.EntryId })
				.HasPrincipalKey<Entry>(x => new { x.SessionId, x.EntryId });
			modelBuilder.Entity<Lap>()
				.HasOne(x => x.Car)
				.WithMany(x => x.Laps)
				.HasForeignKey(x => new { x.SessionId, x.CarId })
				.HasPrincipalKey(x => new { x.SessionId, x.CarId });

			var vehicleData = VehicleSeedData.GetData();
			modelBuilder.Entity<VehicleModel>()
				.HasData(vehicleData.models);
			modelBuilder.Entity<Vehicle>()
				.HasData(vehicleData.vehicles);
			modelBuilder.Entity<VehicleDriver>()
				.HasData(vehicleData.drivers);
		}
	}
}
