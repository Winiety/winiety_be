using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Rides.Core.Services;
using System.Threading.Tasks;

namespace Rides.Core.Consumers
{
    public class GetRideDatesConsumer : IConsumer<GetRideDates>
    {
        private readonly IRideService _rideService;

        public GetRideDatesConsumer(IRideService rideService)
        {
            _rideService = rideService;
        }

        public async Task Consume(ConsumeContext<GetRideDates> context)
        {
            var rides = await _rideService.GetRidesForStatisticsAsync(context.Message.DateFrom, context.Message.DateTo);

            await context.RespondAsync<GetRideDatesResult>(new
            {
                Rides = rides
            });
        }
    }
}
