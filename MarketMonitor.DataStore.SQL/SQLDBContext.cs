using MarketMonitor.RI.Contracts.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MarketMonitor.DataStore.SQL
{
    public class SQLDBContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public SQLDBContext(DbContextOptions<SQLDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DbMapping>()
                .HasOne(e => e.DbAsset)
                .WithMany(e => e.DbMappings)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<DbPrice>()
                .HasOne(e => e.DbAsset)
                .WithMany(e => e.DbPrices)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<DbAsset> Assets { get; set; }
        public DbSet<DbMapping> Mappings { get; set; }
        public DbSet<DbPrice> Prices { get; set; }
    }
}