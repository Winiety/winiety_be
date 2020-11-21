using AutoMapper;
using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Pictures.Core.Interfaces;
using Pictures.Core.Model.DTOs;
using Pictures.Core.Model.Entities;
using Pictures.Core.Services;
using Shared.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pictures.UnitTests.PictureServiceUnitTests
{
    public class PictureServiceSetup
    {
        protected readonly Mock<IPictureRepository> _pictureRepository;
        protected readonly Mock<IBusControl> _bus;
        protected readonly Mock<IRequestClient<AnalyzePicture>> _requestClient;
        protected readonly Mock<IBlobStorageService> _blobStorageService;
        protected readonly Mock<IMapper> _mapper;
        protected readonly Mock<ILogger<PictureService>> _logger;
        protected readonly PictureService _pictureService;

        public PictureServiceSetup()
        {
            _pictureRepository = new Mock<IPictureRepository>();
            _blobStorageService = new Mock<IBlobStorageService>();
            _bus = new Mock<IBusControl>();
            _requestClient = new Mock<IRequestClient<AnalyzePicture>>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<PictureService>>();
            _pictureService = new PictureService(_pictureRepository.Object, _blobStorageService.Object, _bus.Object, _requestClient.Object, _mapper.Object, _logger.Object);
        }


        public IEnumerable<Picture> GetPictures()
        {
            return new List<Picture>
            {
                new Picture
                {
                    Id = 1,
                    PlateNumber = "112233",
                    ImagePath = "path",
                    IsRecognized = true
                },
                new Picture
                {
                    Id = 1,
                    PlateNumber = "112233",
                    ImagePath = "path",
                    IsRecognized = true
                },
                new Picture
                {
                    Id = 1,
                    PlateNumber = "112233",
                    ImagePath = "path",
                    IsRecognized = false
                },
                new Picture
                {
                    Id = 1,
                    PlateNumber = "112233",
                    ImagePath = "path",
                    IsRecognized = false
                }
            };
        }

        public IEnumerable<PictureDTO> GetPicturesDTO(IEnumerable<Picture> pictures)
        {
            return pictures.Select(c => new PictureDTO
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                IsRecognized = c.IsRecognized,
                ImagePath = c.ImagePath
            });
        }

        public class PagedList<T> : List<T>, IPagedList<T>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
        }

        public class AnalyzePictureResultMock : AnalyzePictureResult
        {
            public string PlateNumber { get; set; }
        }

        public class ResponseMock<TResponse> : Response<TResponse> where TResponse : class
        {
            public ResponseMock(TResponse response)
            {
                Message = response;
            }

            public TResponse Message { get; }

            public Guid? MessageId { get; }

            public Guid? RequestId { get; }

            public Guid? CorrelationId { get; }

            public Guid? ConversationId { get; }

            public Guid? InitiatorId { get; }

            public DateTime? ExpirationTime { get; }

            public Uri SourceAddress { get; }

            public Uri DestinationAddress { get; }

            public Uri ResponseAddress { get; }

            public Uri FaultAddress { get; }

            public DateTime? SentTime { get; }

            public Headers Headers { get; }

            public HostInfo Host { get; }
        }
    }
}
