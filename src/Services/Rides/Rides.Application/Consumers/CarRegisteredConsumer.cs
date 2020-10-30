using Contracts.Events;
using MassTransit;
using Rides.Application.Services;
using System.Threading.Tasks;

namespace Rides.Application.Consumers
{
    public class CarRegisteredConsumer : IConsumer<CarRegistered>
    {
        private readonly IRidesService _ridesService;

        public CarRegisteredConsumer(IRidesService ridesService)
        {
            _ridesService = ridesService;
        }

        public async Task Consume(ConsumeContext<CarRegistered> context)
        {
            await _ridesService.RegisterRide(context.Message);
        }
    }
}
