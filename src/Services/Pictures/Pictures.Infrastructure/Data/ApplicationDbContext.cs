using Microsoft.EntityFrameworkCore;
using Pictures.Infrastructure.Data.EntityConfigurations;

namespace Pictures.Infrastructure.Data
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

            builder.ApplyConfiguration(new PictureConfiguration());
        }
    }
}
