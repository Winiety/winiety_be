using Microsoft.EntityFrameworkCore;
using Profile.Core.Models.Entities;
using Profile.Infrastructure.Data.EntityConfigurations;

namespace Profile.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProfileConfiguration());
            builder.ApplyConfiguration(new CarConfiguration());
        }
    }
}
