using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using System.Threading.Tasks;

namespace AI.Application.Consumers
{
    public class AnalyzePictureConsumer : IConsumer<AnalyzePicture>
    {
        public async Task Consume(ConsumeContext<AnalyzePicture> context)
        {
            await context.RespondAsync<AnalyzePictureResult>(new
            {
                PlateNumber = "123456789"
            });
        }
    }
}
