using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pictures.Core.Model;

namespace Pictures.Infrastructure.Data.EntityConfigurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable("Pictures");

            builder.Property(c => c.ImagePath).IsRequired();
        }
    }
}
