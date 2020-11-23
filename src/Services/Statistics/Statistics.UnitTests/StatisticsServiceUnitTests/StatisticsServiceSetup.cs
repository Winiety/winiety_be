using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Moq;
using Statistics.Core.Services;
using System;
using System.Collections.Generic;

namespace Rides.UnitTests.RideServiceUnitTests
{
    public class StatisticsServiceSetup
    {
        protected readonly Mock<IRequestClient<GetRides>> _requestClient;
        protected readonly StatisticsService _statisticsService;

        public StatisticsServiceSetup()
        {
            _requestClient = new Mock<IRequestClient<GetRides>>();
            _statisticsService = new StatisticsService( _requestClient.Object);
        }

        public IEnumerable<DateTimeOffset> GetStatistics()
        {
            return new List<DateTimeOffset>
            {
                DateTimeOffset.UtcNow.AddDays(-4).Date,
                DateTimeOffset.UtcNow.AddDays(-3).Date,
                DateTimeOffset.UtcNow.AddDays(-2).Date,
                DateTimeOffset.UtcNow.AddDays(-1).Date,
            };
        }

        public class GetRidesResultMock : GetRidesResult
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
