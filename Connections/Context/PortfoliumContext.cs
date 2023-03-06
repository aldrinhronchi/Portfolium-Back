using Microsoft.EntityFrameworkCore;

namespace Portfolium_Back.Context
{
    public class PortfoliumContext : DbContext
    {
        public PortfoliumContext(DbContextOptions<PortfoliumContext> option)
            : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyGlobalConfigurations();
            //modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}