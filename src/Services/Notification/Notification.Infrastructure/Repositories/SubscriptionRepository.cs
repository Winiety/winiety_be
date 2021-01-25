using Notification.Core.Interfaces;
using Notification.Core.Model.Entities;
using Notification.Infrastructure.Data;
using Shared.Infrastructure;

namespace Notification.Infrastructure.Repositories
{
    public class SubscriptionRepository : BaseRepository<SubscriptionModel>, ISubscriptionRepository
    {
        public SubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
