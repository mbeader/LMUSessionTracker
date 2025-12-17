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

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
		}
	}
}
