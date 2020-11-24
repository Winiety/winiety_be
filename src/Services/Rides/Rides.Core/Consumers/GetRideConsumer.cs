using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Rides.Core.Services;
using System;
using System.Threading.Tasks;

namespace Rides.Core.Consumers
{
    public class GetRideConsumer : IConsumer<GetRide>
    {
        private readonly IRideService _rideService;

        public GetRideConsumer(IRideService rideService)
        {
            _rideService = rideService;
        }

        public async Task Consume(ConsumeContext<GetRide> context)
        {
            var ride = await _rideService.GetRideForFinesAsync(context.Message.RideId);

            if (ride == null)
            {
                await context.RespondAsync<GetRideNotFound>(new
                {
                    RideId = context.Message.RideId
                });
            }
            else
            {
                await context.RespondAsync<GetRideResult>(new
                {
                    Id = ride.Id,
                    UserId = ride.UserId,
                    PictureId = ride.PictureId,
                    PlateNumber = ride.PlateNumber,
                    RideDateTime = ride.RideDateTime
                });
            }
        }
    }
}
