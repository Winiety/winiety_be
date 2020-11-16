using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Models.Entities;

namespace Profile.Infrastructure.Data.EntityConfigurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");

            builder.Property(c => c.PlateNumber).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
        }
    }
}
