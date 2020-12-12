using Fines.Core.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fines.Infrastructure.Data.EntityConfigurations
{
    public class FineConfiguration : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> builder)
        {
            builder.ToTable("Fines");

            builder.Property(c => c.PictureId).IsRequired();
            builder.Property(c => c.RideId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.PlateNumber).IsRequired();
        }
    }
}
