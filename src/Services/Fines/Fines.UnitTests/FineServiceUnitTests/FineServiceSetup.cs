using AutoMapper;
using Contracts.Commands;
using Contracts.Results;
using Fines.Core.Interfaces;
using Fines.Core.Model.DTOs.Fine;
using Fines.Core.Model.Entities;
using Fines.Core.Services;
using MassTransit;
using Moq;
using Shared.Core.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fines.UnitTests.FineServiceUnitTests
{
    public class FineServiceSetup
    {
        protected readonly Mock<IFineRepository> _fineRepository;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IBusControl> _bus;
        protected readonly Mock<IRequestClient<GetRide>> _requestClient;
        protected readonly Mock<IMapper> _mapper;
        protected readonly FineService _fineService;

        public FineServiceSetup()
        {
            _fineRepository = new Mock<IFineRepository>();
            _bus = new Mock<IBusControl>();
            _userContext = new Mock<IUserContext>();
            _requestClient = new Mock<IRequestClient<GetRide>>();
            _mapper = new Mock<IMapper>();
            _fineService = new FineService(_fineRepository.Object, _bus.Object, _userContext.Object, _requestClient.Object, _mapper.Object);
        }

        public IEnumerable<Fine> GetFines()
        {
            return new List<Fine>
            {
                new Fine
                {
                    Id = 1,
                    PlateNumber = "112233",
                    UserId = 1,
                    Cost = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Fine
                {
                    Id = 2,
                    PlateNumber = "112233",
                    UserId = 1,
                    Cost = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Fine
                {
                    Id = 3,
                    PlateNumber = "112233",
                    UserId = 2,
                    Cost = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Fine
                {
                    Id = 4,
                    PlateNumber = "112233",
                    UserId = 2,
                    Cost = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                }
            };
        }

        public IEnumerable<FineDTO> GetFinesDTO(IEnumerable<Fine> fines)
        {
            return fines.Select(c => new FineDTO
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                CreateTime = c.CreateTime,
                PictureId = c.PictureId,
                RideId = c.RideId,
                Description = c.Description,
                Cost = c.Cost
            });
        }

        public IEnumerable<FineDetailDTO> GetFineDetailsDTO(IEnumerable<Fine> fines)
        {
            return fines.Select(c => new FineDetailDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                PlateNumber = c.PlateNumber,
                CreateTime = c.CreateTime,
                PictureId = c.PictureId,
                RideId = c.RideId,
                Description = c.Description,
                Cost = c.Cost
            });
        }

        public class PagedList<T> : List<T>, IPagedList<T>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
        }

        public class GetRideResultMock : GetRideResult
        {
            public int Id { get; set; }
            public int? UserId { get; set; }
            public int PictureId { get; set; }
            public string PlateNumber { get; set; }
            public DateTimeOffset RideDateTime { get; set; }
        }

        public class GetRideNotFoundMock : GetRideNotFound
        {
            public int RideId { get; set; }
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
