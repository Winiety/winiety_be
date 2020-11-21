using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using MassTransit.Testing;
using Profile.Core.Consumers;
using System.Threading.Tasks;
using Xunit;

namespace Profile.UnitTests.CarServiceUnitTests
{
    public class CarServiceBusTests : CarServiceSetup
    {

        [Fact]
        public async Task GetUserIdByPlateConsumer_Should_ConsumeGetUserIdByPlate()
        {
            var responseMock = new ResponseMock<GetUserIdByPlateResult>(new GetUserIdByPlateResultMock
            {
                UserId = 1
            });

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new GetUserIdByPlateConsumer(_carService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<GetUserIdByPlate>(new
                {
                    PlateNumber = "112233"
                });

                Assert.True(await harness.Consumed.Any<GetUserIdByPlate>());
                Assert.True(await consumerHarness.Consumed.Any<GetUserIdByPlate>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
