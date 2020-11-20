using Contracts.Events;
using MassTransit;
using Notification.Core.Services;
using System.Threading.Tasks;

namespace Notification.Core.Consumers
{
    public class RideRegisteredConsumer : IConsumer<RideRegistered>
    {
        private readonly INotificationService _notificationService;

        public RideRegisteredConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<RideRegistered> context)
        {
            if (context.Message.UserId.HasValue)
            {
                await _notificationService.CreateNotificationAsync(context.Message);
            }
        }
    }
}
