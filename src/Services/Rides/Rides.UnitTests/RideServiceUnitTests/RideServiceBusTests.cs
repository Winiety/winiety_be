using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Rides.Core.Consumers;
using Rides.Core.Model.Entities;
using System;
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
        public async Task CarRegisteredConsumer_Should_ConsumeCarRegistered()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new CarRegisteredConsumer(_rideService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<CarRegistered>(new
                {
                    PictureId = default(int),
                    PlateNumber = default(string)
                });

                Assert.True(await harness.Consumed.Any<CarRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<CarRegistered>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task GetRideDatesConsumer_Should_ConsumeGetRideDates()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new GetRideDatesConsumer(_rideService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<GetRideDates>(new
                {
                    DateFrom = default(DateTimeOffset),
                    DateTo = default(DateTimeOffset)
                });

                Assert.True(await harness.Consumed.Any<GetRideDates>());
                Assert.True(await consumerHarness.Consumed.Any<GetRideDates>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task GetRideConsumer_Should_ConsumeGetRide()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new GetRideConsumer(_rideService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<GetRide>(new
                {
                    RideId = default(int)
                });

                Assert.True(await harness.Consumed.Any<GetRide>());
                Assert.True(await consumerHarness.Consumed.Any<GetRide>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
