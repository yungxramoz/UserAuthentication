using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAuthentication.Data.Entities;

namespace UserAuthentication.Data
{
    public class UserContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DbSet<User> Users { get; set; }

        public UserContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("UserDatabase");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
