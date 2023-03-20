using Microsoft.EntityFrameworkCore;
using Portfolium_Back.Connections.Configurations;
using Portfolium_Back.Extensions;
using Portfolium_Back.Models;

namespace Portfolium_Back.Context
{
    public class PortfoliumContext : DbContext
    {
        public PortfoliumContext(DbContextOptions<PortfoliumContext> option)
            : base(option) { }

        #region DBSets

        public DbSet<User> Users { get; set; }

        #endregion DBSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserConfiguration());

            //modelBuilder.ApplyGlobalConfigurations();
            //modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}