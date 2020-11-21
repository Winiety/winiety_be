using Contracts.Events;
using Contracts.Results;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Rides.Core.Consumers;
using Rides.Core.Model.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Rides.UnitTests.RideServiceUnitTests
{
    public class RideServiceBusTests : RideServiceSetup
    {
        [Fact]
        public async Task RegisterRideAsync_Should_PublishRideRegistered()
        {
            var responseMock = new ResponseMock<GetUserIdByPlateResult>(new GetUserIdByPlateResultMock
            {
                UserId = 1
            });

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new CarRegisteredConsumer(_rideService));

            _requestClient
               .Setup(c => c.GetResponse<GetUserIdByPlateResult>(It.IsAny<object>(), default, default))
               .ReturnsAsync(responseMock);

            _rideRepository
              .Setup(c => c.AddAsync(It.IsAny<Ride>()));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<CarRegistered>(new
                {

                    PictureId = 1,
                    PlateNumber = "112233"
                });

                await harness.Consumed.Any<CarRegistered>();

                _bus.Verify(c => c.Publish<RideRegistered>(It.IsAny<object>(), default), Times.Once());
                Assert.False(await harness.Published.Any<Fault<CarRegistered>>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task RegisterRideAsync_Should_ConsumeCarRegistered()
        {
            var responseMock = new ResponseMock<GetUserIdByPlateResult>(new GetUserIdByPlateResultMock
            {
                UserId = 1
            });

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new CarRegisteredConsumer(_rideService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<CarRegistered>(new
                {

                    PictureId = 1,
                    PlateNumber = "112233"
                });

                Assert.True(await harness.Consumed.Any<CarRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<CarRegistered>());

            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
