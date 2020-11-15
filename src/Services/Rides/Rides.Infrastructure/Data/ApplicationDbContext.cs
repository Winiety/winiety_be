using Microsoft.EntityFrameworkCore;
using Rides.Infrastructure.Data.EntityConfigurations;

namespace Rides.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RideConfiguration());
        }
    }
}
