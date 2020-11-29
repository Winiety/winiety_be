using Contracts.Events;
using MassTransit;
using Notification.Core.Services;
using System.Threading.Tasks;

namespace Notification.Core.Consumers
{
    public class ComplaintRegisteredConsumer : IConsumer<ComplaintRegistered>
    {
        private readonly INotificationService _notificationService;

        public ComplaintRegisteredConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<ComplaintRegistered> context)
        {
            await _notificationService.CreateNotificationAsync(context.Message);
        }
    }
}
