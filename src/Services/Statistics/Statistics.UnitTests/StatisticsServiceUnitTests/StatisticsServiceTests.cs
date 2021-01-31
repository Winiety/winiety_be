using Contracts.Results;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Statistics.Core.Model.DTOs;
using Statistics.Core.Model.Requests;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Rides.UnitTests.RideServiceUnitTests
{
    public class StatisticsServiceTests : StatisticsServiceSetup
    {
        [Fact]
        public async Task GetCsvDataStatistics_Should_CsvStatistics()
        {
            var stats = GetStatistics();
            var searchRequest = new GetStatisticsRequest
            {
                DateFrom = DateTimeOffset.UtcNow.AddDays(-4).Date,
                DateTo = DateTimeOffset.UtcNow.AddDays(1).Date,
                GroupBy = GroupByType.Day
            };

            var expected = new StringBuilder();

            expected.AppendLine("Day;Rides number");

            foreach (var stat in stats)
            {
                expected.AppendLine($"{stat.ToString("dd MMM")};1");
            }
            expected.AppendLine($"{DateTimeOffset.UtcNow.ToString("dd MMM")};0");

            var responseMock = new ResponseMock<GetRideDatesResult>(new GetRidesResultMock
            {
                Rides = stats
            });

            _requestClient
                .Setup(c => c.GetResponse<GetRideDatesResult>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync(responseMock);

            var result = await _statisticsService.GetCsvDataStatistics(searchRequest);

            Assert.Equal(expected.ToString(), result);
        }

        [Fact]
        public async Task GetJsonDataStatistics_Should_JsonStatistics()
        {
            var stats = GetStatistics();
            var searchRequest = new GetStatisticsRequest
            {
                DateFrom = DateTimeOffset.UtcNow.AddDays(-4).Date,
                DateTo = DateTimeOffset.UtcNow.AddDays(1).Date,
                GroupBy = GroupByType.Day
            };

            var data = stats.Select(c => new JsonStatsElement
            {
                Label = c.ToString("dd MMM"),
                Value = 1
            }).ToList();

            data.Add(new JsonStatsElement
            {
                Label = DateTimeOffset.UtcNow.ToString("dd MMM"),
                Value = 0
            });

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var expected = JsonSerializer.Serialize(new JsonStatsDTO
            {
                Data = data
            }, options);

            var responseMock = new ResponseMock<GetRideDatesResult>(new GetRidesResultMock
            {
                Rides = stats
            });

            _requestClient
                .Setup(c => c.GetResponse<GetRideDatesResult>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync(responseMock);

            var result = await _statisticsService.GetJsonDataStatistics(searchRequest);

            Assert.Equal(expected.ToString(), result);
        }

        [Fact]
        public async Task GetChartStatistics_Should_ChartStatistics()
        {
            var stats = GetStatistics();
            var searchRequest = new GetStatisticsRequest
            {
                DateFrom = DateTimeOffset.UtcNow.AddDays(-4).Date,
                DateTo = DateTimeOffset.UtcNow.AddDays(1).Date,
                GroupBy = GroupByType.Day
            };

            var responseMock = new ResponseMock<GetRideDatesResult>(new GetRidesResultMock
            {
                Rides = stats
            });

            _requestClient
                .Setup(c => c.GetResponse<GetRideDatesResult>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync(responseMock);

            var result = await _statisticsService.GetChartStatistics(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal("Rides Per Day", result.Result.XTitle);
            Assert.Equal("Rides number", result.Result.YTitle);
            Assert.Equal(5, result.Result.XValues.Count());
            Assert.Equal(5, result.Result.YValues.Count());
        }
    }
}
