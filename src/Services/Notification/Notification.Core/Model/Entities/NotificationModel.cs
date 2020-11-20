using Shared.Core.BaseModels.Entities;
using System;

namespace Notification.Core.Model.Entities
{
    public class NotificationModel : BaseEntity
    {
        public int UserId { get; set; }
        public string NotificationType { get; set; }
        public int RedirectId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}
