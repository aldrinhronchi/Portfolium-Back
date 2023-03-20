using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.ID).IsRequired();

            builder.Property(x => x.GuidID).IsRequired().HasDefaultValue(Guid.NewGuid());

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Password).IsRequired().HasDefaultValue("System");

            builder.Property(x => x.Role).HasDefaultValue("admin");
        }
    }
}