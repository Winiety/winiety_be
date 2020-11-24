using Fines.Core.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fines.Infrastructure.Data.EntityConfigurations
{
    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.ToTable("Complaints");

            builder.Property(c => c.PictureId).IsRequired();
            builder.Property(c => c.RideId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.PlateNumber).IsRequired();
        }
    }
}
