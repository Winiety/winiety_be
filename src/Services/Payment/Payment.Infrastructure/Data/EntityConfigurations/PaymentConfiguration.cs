using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Infrastructure.Data.EntityConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Core.Model.Entities.Payment>
    {
        public void Configure(EntityTypeBuilder<Core.Model.Entities.Payment> builder)
        {
            builder.ToTable("Payments");

            builder.Property(c => c.RideId).IsRequired();
            builder.Property(c => c.Amount).IsRequired();
        }
    }
}
