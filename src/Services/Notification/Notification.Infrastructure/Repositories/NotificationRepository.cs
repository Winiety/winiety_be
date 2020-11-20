using Notification.Core.Interfaces;
using Notification.Core.Model.Entities;
using Notification.Infrastructure.Data;
using Shared.Infrastructure;

namespace Notification.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository<NotificationModel>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
