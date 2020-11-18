using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rides.Core.Model.Entities;

namespace Rides.Infrastructure.Data.EntityConfigurations
{
    public class RideConfiguration : IEntityTypeConfiguration<Ride>
    {
        public void Configure(EntityTypeBuilder<Ride> builder)
        {
            builder.ToTable("Rides");

            builder.Property(c => c.PictureId).IsRequired();
            builder.Property(c => c.PlateNumber).IsRequired();
            builder.Property(c => c.RideDateTime).IsRequired();
        }
    }
}
