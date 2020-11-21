using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Core.Model.Entities;

namespace Notification.Infrastructure.Data.EntityConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationModel>
    {
        public void Configure(EntityTypeBuilder<NotificationModel> builder)
        {
            builder.ToTable("Notifications");

            builder.Property(c => c.UserId).IsRequired();
        }
    }
}
