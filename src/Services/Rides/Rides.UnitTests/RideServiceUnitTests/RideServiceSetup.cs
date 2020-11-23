using AutoMapper;
using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Rides.Core.Interfaces;
using Rides.Core.Model.DTOs;
using Rides.Core.Model.Entities;
using Rides.Core.Services;
using Shared.Core.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rides.UnitTests.RideServiceUnitTests
{
    public class RideServiceSetup
    {
        protected readonly Mock<IRideRepository> _rideRepository;
        protected readonly Mock<IBusControl> _bus;
        protected readonly Mock<IRequestClient<GetUserIdByPlate>> _requestClient;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IMapper> _mapper;
        protected readonly Mock<ILogger<RideService>> _logger;
        protected readonly RideService _rideService;

        public RideServiceSetup()
        {
            _rideRepository = new Mock<IRideRepository>();
            _bus = new Mock<IBusControl>();
            _requestClient = new Mock<IRequestClient<GetUserIdByPlate>>();
            _userContext = new Mock<IUserContext>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<RideService>>();
            _rideService = new RideService(_rideRepository.Object, _bus.Object, _requestClient.Object, _userContext.Object, _mapper.Object, _logger.Object);
        }

        public IEnumerable<Ride> GetRides()
        {
            return new List<Ride>
            {
                new Ride
                {
                    Id = 1,
                    PictureId = 1,
                    PlateNumber = "112233",
                    RideDateTime = DateTimeOffset.UtcNow.AddDays(-1),
                    UserId = 1
                },
                new Ride
                {
                    Id = 2,
                    PictureId = 2,
                    PlateNumber = "112233",
                    RideDateTime = DateTimeOffset.UtcNow.AddDays(-2),
                    UserId = 1
                },
                new Ride
                {
                    Id = 3,
                    PictureId = 3,
                    PlateNumber = "112233",
                    RideDateTime = DateTimeOffset.UtcNow.AddDays(-3),
                    UserId = 2
                },
                new Ride
                {
                    Id = 4,
                    PictureId = 4,
                    PlateNumber = "112233",
                    RideDateTime = DateTimeOffset.UtcNow.AddDays(-4),
                    UserId = 2
                }
            };
        }

        public IEnumerable<RideDTO> GetRidesDTO(IEnumerable<Ride> rides)
        {
            return rides.Select(c => new RideDTO
            {
                Id = c.Id,
                PictureId = c.PictureId,
                PlateNumber = c.PlateNumber,
                RideDateTime = c.RideDateTime
            });
        }

        public IEnumerable<RideDetailDTO> GetRidesDetailDTO(IEnumerable<Ride> rides)
        {
            return rides.Select(c => new RideDetailDTO
            {
                Id = c.Id,
                PictureId = c.PictureId,
                PlateNumber = c.PlateNumber,
                RideDateTime = c.RideDateTime,
                UserId=c.UserId
            });
        }

        public class PagedList<T> : List<T>, IPagedList<T>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
        }

        public class GetUserIdByPlateResultMock : GetUserIdByPlateResult
        {
            public int? UserId { get; set; }
        }

        public class GetRidesResultResultMock : GetRidesResult
        {
            public IEnumerable<DateTimeOffset> Rides { get; set; }
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
