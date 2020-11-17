using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Pictures.Core.Interfaces;
using Pictures.Core.Model;
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
        private readonly IPictureRepository _pictureRepository;
        private readonly IBusControl _bus;
        private readonly ILogger<PictureService> _logger;

        public PictureService(IPictureRepository pictureRepository, IBusControl bus, ILogger<PictureService> logger)
        {
            _pictureRepository = pictureRepository;
            _bus = bus;
            _logger = logger;
        }

        public async Task<string> AddPictureAsync(byte[] pictureData)
        {
            var uri = new Uri("rabbitmq://localhost/ai-listener");

            var requestClient = _bus.CreateRequestClient<AnalyzePicture>(uri);

            var response = await requestClient.GetResponse<AnalyzePictureResult>(new
            {
                Data = pictureData
            });

            var plateNumber = response.Message.PlateNumber;

            var picture = new Picture
            {
                IsRecognized = string.IsNullOrWhiteSpace(plateNumber),
                PlateNumber = plateNumber,
                ImagePath = plateNumber
            };

            await _pictureRepository.AddAsync(picture);

            _logger.LogInformation($"Picture registered - [ID={picture.Id}] [PlateNumber={picture.PlateNumber}] [ImagePath={picture.ImagePath}] [IsRecognized={picture.IsRecognized}]");

            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                return "Not recognized";
            }

            await _bus.Publish<CarRegistered>(new
            {
                PictureId = picture.Id,
                PlateNumber = picture.PlateNumber
            });

            return response.Message.PlateNumber;
        }
    }
}
