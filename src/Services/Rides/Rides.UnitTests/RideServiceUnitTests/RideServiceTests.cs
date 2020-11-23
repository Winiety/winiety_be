using Contracts.Results;
using MassTransit.Testing;
using Moq;
using Rides.Core.Model.DTOs;
using Rides.Core.Model.Entities;
using Rides.Core.Model.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rides.UnitTests.RideServiceUnitTests
{
    public class RideServiceTests : RideServiceSetup
    {
        [Fact]
        public async Task GetRidesAsync_Should_ReturnRides()
        {
            var rides = GetRides();
            var searchRequest = new RideSearchRequest();
            var paged = new PagedList<Ride>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = rides.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<RideDetailDTO>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = rides.Count(),
                PageSize = searchRequest.PageSize,
                Results = GetRidesDetailDTO(rides)
            };

            paged.AddRange(rides);

            _rideRepository
                .Setup(c => c.GetQueryable())
                .Returns(rides.AsQueryable());

            _rideRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Ride>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Ride>>(), It.IsAny<PagedResponse<RideDetailDTO>>()))
                .Returns(response);

            var result = await _rideService.GetRidesAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(rides.Count(), result.TotalCount);
        }

        [Fact]
        public async Task GetUserRidesAsync_Should_ReturnUserRides()
        {
            var rides = GetRides();
            var searchRequest = new RideSearchRequest();
            var paged = new PagedList<Ride>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = rides.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<RideDTO>()
            {
                TotalPages = paged.TotalPages,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = paged.TotalCount,
                PageSize = searchRequest.PageSize,
                Results = GetRidesDTO(rides)
            };

            paged.AddRange(rides);

            _rideRepository
                .Setup(c => c.GetQueryable())
                .Returns(rides.AsQueryable());

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _rideRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Ride>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Ride>>(), It.IsAny<PagedResponse<RideDTO>>()))
                .Returns(response);

            var result = await _rideService.GetUserRidesAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(rides.Count(), result.TotalCount);
            _userContext.Verify(c => c.GetUserId(), Times.Once());
        }

        [Fact]
        public async Task RegisterRideAsync_Should_AddRegister()
        {
            var responseMock = new ResponseMock<GetUserIdByPlateResult>(new GetUserIdByPlateResultMock
            {
                UserId = 1
            });

            _requestClient
                .Setup(c => c.GetResponse<GetUserIdByPlateResult>(It.IsAny<object>(), default, default))
                .ReturnsAsync(responseMock);

            _rideRepository
                .Setup(c => c.AddAsync(It.IsAny<Ride>()));

            await _rideService.RegisterRideAsync(1, "112233");

            _rideRepository.Verify(c => c.AddAsync(It.IsAny<Ride>()), Times.Once());
        }
    }
}
