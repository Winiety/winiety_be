using Fines.Core.Model.Entities;
using Fines.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Fines.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Complaint> Complaints { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new FineConfiguration());
            builder.ApplyConfiguration(new ComplaintConfiguration());
        }
    }
}
