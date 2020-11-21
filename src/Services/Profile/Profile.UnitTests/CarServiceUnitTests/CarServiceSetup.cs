using AutoMapper;
using Contracts.Results;
using MassTransit;
using Moq;
using Profile.Core.Interfaces;
using Profile.Core.Models.DTOs.Car;
using Profile.Core.Models.Entities;
using Profile.Core.Services;
using Shared.Core.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Profile.UnitTests.CarServiceUnitTests
{
    public class CarServiceSetup
    {
        protected readonly Mock<ICarRepository> _carRepository;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IMapper> _mapper;
        protected readonly CarService _carService;

        public CarServiceSetup()
        {
            _carRepository = new Mock<ICarRepository>();
            _userContext = new Mock<IUserContext>();
            _mapper = new Mock<IMapper>();
            _carService = new CarService(_carRepository.Object, _userContext.Object, _mapper.Object);
        }

        public IEnumerable<Car> GetCars()
        {
            return new List<Car>
            {
                new Car
                {
                    Id = 1,
                    PlateNumber = "112233",
                    UserId = 1
                },
                new Car
                {
                    Id = 2,
                    PlateNumber = "112233",
                    UserId = 1
                },
                new Car
                {
                    Id = 3,
                    PlateNumber = "112233",
                    UserId = 2
                },
                new Car
                {
                    Id = 4,
                    PlateNumber = "112233",
                    UserId = 2
                }
            };
        }

        public IEnumerable<CarDTO> GetCarsDTO(IEnumerable<Car> cars)
        {
            return cars.Select(c => new CarDTO
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
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
