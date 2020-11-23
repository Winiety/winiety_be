using Shared.Core.BaseModels.Responses.Interfaces;
using System;

namespace Notification.Core.Model.DTOs
{
    public class NotificationDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string NotificationType { get; set; }
        public int RedirectId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}
