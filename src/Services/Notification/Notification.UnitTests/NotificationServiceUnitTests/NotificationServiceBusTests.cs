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
                    Id = default(int),
                    UserId = default(int?),
                    PlateNumber = default(string),
                    RideDateTime = default(DateTimeOffset)
                });

                Assert.True(await harness.Consumed.Any<RideRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<RideRegistered>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task FineRegisteredConsumer_Should_ConsumeFineRegistered()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new FineRegisteredConsumer(_notificationService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<FineRegistered>(new
                {
                    Id = default(int),
                    UserId = default(int),
                    RideId = default(int),
                    Cost = default(double),
                    Description = default(string),
                    CreateDateTime = default(DateTimeOffset)
                });

                Assert.True(await harness.Consumed.Any<FineRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<FineRegistered>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task ComplaintRegisteredConsumer_Should_ConsumeComplaintRegistered()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new ComplaintRegisteredConsumer(_notificationService));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<ComplaintRegistered>(new
                {
                    Id = default(int),
                    UserId = default(int),
                    RideId = default(int),
                    Description = default(string),
                    CreateDateTime = default(DateTimeOffset)
                });

                Assert.True(await harness.Consumed.Any<ComplaintRegistered>());
                Assert.True(await consumerHarness.Consumed.Any<ComplaintRegistered>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
