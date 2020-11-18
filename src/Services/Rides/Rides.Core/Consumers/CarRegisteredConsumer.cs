using Contracts.Events;
using MassTransit;
using Rides.Core.Services;
using System.Threading.Tasks;

namespace Rides.Core.Consumers
{
    public class CarRegisteredConsumer : IConsumer<CarRegistered>
    {
        private readonly IRideService _ridesService;

        public CarRegisteredConsumer(IRideService ridesService)
        {
            _ridesService = ridesService;
        }

        public async Task Consume(ConsumeContext<CarRegistered> context)
        {
            await _ridesService.RegisterRideAsync(context.Message.PictureId, context.Message.PlateNumber);
        }
    }
}
