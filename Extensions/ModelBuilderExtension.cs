using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Portfolium_Back.Models;
using Portfolium_Back.Models.Entities;

namespace Portfolium_Back.Extensions
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder SeedData(this ModelBuilder builder)
        {
            //builder.Entity<User>()
            //    .HasData(
            //        new User { ID = 4, GuidID = Guid.Parse($"b25fca61-2f00-472a-bcb0-b87e7080b255"), Name = $"Seith Terore", Email = $"work.aldrinronchi@gmail.com", DateCreated = new DateTime(2000, 8, 22), IsActive = true, DateUpdated = null, UserCreated = $"Terore" }
            //    );
            return builder;
        }

        public static ModelBuilder ApplyGlobalConfigurations(this ModelBuilder builder)
        {
            foreach (IMutableEntityType? entityType in builder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty? property in entityType.GetProperties())
                {
                    switch (property.Name)
                    {
                        case nameof(Entity.ID):
                            property.IsKey();
                            break;

                        case nameof(Entity.DateUpdated):
                            property.IsNullable = true;
                            break;

                        case nameof(Entity.DateCreated):
                            property.IsNullable = false;
                            property.SetDefaultValue(DateTime.Now);
                            break;

                        case nameof(Entity.IsActive):
                            property.IsNullable = false;
                            property.SetDefaultValue(true);
                            break;

                        case nameof(Entity.UserCreated):
                            property.IsNullable = true;
                            property.SetDefaultValue("System");
                            break;

                        case nameof(Entity.UserUpdated):
                            property.IsNullable = true;
                            break;

                        default:
                            break;
                    }
                }
            }
            return builder;
        }
    }
}