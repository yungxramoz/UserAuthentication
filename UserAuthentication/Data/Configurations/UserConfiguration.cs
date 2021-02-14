using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAuthentication.Data.Entities;

namespace UserAuthentication.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasData(
                new User
                {
                    PersonId = 1,
                    Username = "admin",
                    Password = "admin",
                    Firstname = "Admin",
                    Lastname = "Admin",
                    IsAdministrator = true,
                }
            );
        }
    }
}