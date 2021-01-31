using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Infrastructure.Data.EntityConfigurations
{
    public class WinietaConfiguration : IEntityTypeConfiguration<Core.Model.Entities.Winieta>
    {
        public void Configure(EntityTypeBuilder<Core.Model.Entities.Winieta> builder)
        {
            builder.ToTable("Winietas");

            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Cost).IsRequired();
            builder.Property(c => c.ExpirationDate).IsRequired();
        }
    }
}
