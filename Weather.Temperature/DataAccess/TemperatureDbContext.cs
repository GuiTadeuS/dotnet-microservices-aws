using Microsoft.EntityFrameworkCore;

namespace Weather.Temperature.DataAccess
{
    public class TemperatureDbContext : DbContext
    {

        public TemperatureDbContext()
        {
        }

        public TemperatureDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Temperature> Temperatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SnakeCaseIdentityTableNames(modelBuilder);
        }

        private static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temperature>(b => { b.ToTable("temperature"); });
        }
    }
}
