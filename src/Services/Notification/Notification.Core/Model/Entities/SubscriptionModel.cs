using Shared.Core.BaseModels.Entities;

namespace Notification.Core.Model.Entities
{
    public class SubscriptionModel : BaseEntity
    {
        public int UserId { get; set; }
        public string Endpoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
