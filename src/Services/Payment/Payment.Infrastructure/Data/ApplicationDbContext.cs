using Microsoft.EntityFrameworkCore;

namespace Payment.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Core.Model.Entities.Payment> Payments { get; set; }
        public DbSet<Core.Model.Entities.Winieta> Winietas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EntityConfigurations.PaymentConfiguration());
            builder.ApplyConfiguration(new EntityConfigurations.WinietaConfiguration());
        }
    }
}
