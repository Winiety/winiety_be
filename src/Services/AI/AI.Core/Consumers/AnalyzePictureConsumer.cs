using AI.Core.Services;
using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using System.Threading.Tasks;

namespace AI.Core.Consumers
{
    public class AnalyzePictureConsumer : IConsumer<AnalyzePicture>
    {
        private readonly IAIService _aIService;

        public AnalyzePictureConsumer(IAIService aIService)
        {
            _aIService = aIService;
        }

        public async Task Consume(ConsumeContext<AnalyzePicture> context)
        {
            var plateNumber = await _aIService.AnalyzePicture(context.Message.ImagePath);

            await context.RespondAsync<AnalyzePictureResult>(new
            {
                PlateNumber = plateNumber
            });
        }
    }
}
