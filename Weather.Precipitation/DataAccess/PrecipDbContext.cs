using Microsoft.EntityFrameworkCore;

namespace Weather.Precipitation.DataAccess
{
    public class PrecipDbContext : DbContext
    {
        public PrecipDbContext(DbContextOptions options) : base(options)
        {
        }

        public PrecipDbContext()
        {
        }
        public DbSet<Precipitation> Precipitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SnakeCaseIdentityTableNames(modelBuilder);
        }

        private static void  SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Precipitation>(b => { b.ToTable("precipitation"); });
        }

    }
}
