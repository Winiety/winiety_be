using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Profile.Core.Services;
using System.Threading.Tasks;

namespace Profile.Core.Consumers
{
    public class GetUserIdByPlateConsumer : IConsumer<GetUserIdByPlate>
    {
        private readonly ICarService _carService;

        public GetUserIdByPlateConsumer(ICarService carService)
        {
            _carService = carService;
        }

        public async Task Consume(ConsumeContext<GetUserIdByPlate> context)
        {
            var userId = await _carService.GetUserIdByPlateAsync(context.Message.PlateNumber);

            await context.RespondAsync<GetUserIdByPlateResult>(new
            {
                UserId = userId
            });
        }
    }
}
