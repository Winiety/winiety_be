using Contracts.Events;
using MassTransit.Testing;
using Notification.Core.Consumers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Notification.UnitTests.NotificationServiceUnitTests
{
    public class NotificationServiceBusTests : NotificationServiceSetup
    {
        [Fact]
        public async Task RideRegisteredConsumer_Should_ConsumeRideRegistered()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new RideRegisteredConsumer(_notificationService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<RideRegistered>(new
                {
                    Id = 1,
                    UserId = 1,
                    PlateNumber = "112233",
                    RideDateTime = DateTimeOffset.UtcNow
                });

                Assert.True(await harness.Consumed.Any<RideRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<RideRegistered>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
