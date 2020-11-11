using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Pictures.Core.Services
{
    public interface IPictureService
    {
        Task<string> AddPictureAsync(byte[] pictureData);
    }

    public class PictureService : IPictureService
    {
        private readonly IBusControl _bus;

        public PictureService(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task<string> AddPictureAsync(byte[] pictureData)
        {
            var uri = new Uri("rabbitmq://localhost/ai-listener");

            var requestClient = _bus.CreateRequestClient<AnalyzePicture>(uri);

            var response = await requestClient.GetResponse<AnalyzePictureResult>(new
            {
                Data = pictureData
            });

            if (string.IsNullOrWhiteSpace(response.Message.PlateNumber))
            {
                return "Not recognized";
            }

            await _bus.Publish<CarRegistered>(new
            {
                PictureId = 1,
                PlateNumber = "1234567"
            });

            return response.Message.PlateNumber;
        }
    }
}
