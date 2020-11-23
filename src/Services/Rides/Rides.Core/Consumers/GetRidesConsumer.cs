using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Rides.Core.Services;
using System.Threading.Tasks;

namespace Rides.Core.Consumers
{
    public class GetRidesConsumer : IConsumer<GetRides>
    {
        private readonly IRideService _rideService;

        public GetRidesConsumer(IRideService rideService)
        {
            _rideService = rideService;
        }

        public async Task Consume(ConsumeContext<GetRides> context)
        {
            var rides = await _rideService.GetRidesForStatisticsAsync(context.Message.DateFrom, context.Message.DateTo);

            await context.RespondAsync<GetRidesResult>(new
            {
                Rides = rides
            });
        }
    }
}
