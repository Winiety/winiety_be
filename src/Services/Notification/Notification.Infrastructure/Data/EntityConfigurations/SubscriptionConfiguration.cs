using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Core.Model.Entities;

namespace Notification.Infrastructure.Data.EntityConfigurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionModel>
    {
        public void Configure(EntityTypeBuilder<SubscriptionModel> builder)
        {
            builder.ToTable("Subscriptions");

            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Auth).IsRequired();
            builder.Property(c => c.Endpoint).IsRequired();
            builder.Property(c => c.P256dh).IsRequired();
        }
    }
}
