using Microsoft.EntityFrameworkCore;
using Portfolium_Back.Extensions;
using Portfolium_Back.Models;
using Portfolium_Back.Models.Entities;

namespace Portfolium_Back.Context
{
    public class PortfoliumContext : DbContext
    {
        public PortfoliumContext(DbContextOptions<PortfoliumContext> option)
            : base(option) { }

        #region DBSets

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Erro> Erros { get; set; }
        public DbSet<Ocorrencia> Ocorrencia { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }  
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Certification> Certifications { get; set; }
      

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