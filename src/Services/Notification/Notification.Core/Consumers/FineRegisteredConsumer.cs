using Contracts.Events;
using MassTransit;
using Notification.Core.Services;
using System.Threading.Tasks;

namespace Notification.Core.Consumers
{
    public class FineRegisteredConsumer : IConsumer<FineRegistered>
    {
        private readonly INotificationService _notificationService;

        public FineRegisteredConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<FineRegistered> context)
        {
            await _notificationService.CreateNotificationAsync(context.Message);
        }
    }
}
