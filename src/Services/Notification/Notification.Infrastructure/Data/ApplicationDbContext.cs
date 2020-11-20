using Microsoft.EntityFrameworkCore;
using Notification.Core.Model.Entities;
using Notification.Infrastructure.Data.EntityConfigurations;

namespace Notification.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<NotificationModel> Pictures { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new NotificationConfiguration());
        }
    }
}
