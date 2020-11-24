using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Statistics.Core.Model.DTOs;
using Statistics.Core.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Statistics.Core.Services
{
    public interface IStatisticsService
    {
        Task<IResultResponse<ChartStatsDTO>> GetChartStatistics(GetStatisticsRequest request);
        Task<string> GetCsvDataStatistics(GetStatisticsRequest request);
        Task<string> GetJsonDataStatistics(GetStatisticsRequest request);
    }

    public class StatisticsService : IStatisticsService
    {
        private const string DayFormat = "dd MMM";
        private const string WeekFormat = "dd MMM yyyy";
        private const string MonthFormat = "MMMM yyyy";
        private const string YearFormat = "yyyy";

        private readonly IRequestClient<GetRideDates> _requestClient;

        public StatisticsService(IRequestClient<GetRideDates> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<string> GetCsvDataStatistics(GetStatisticsRequest request)
        {
            var result = await _requestClient.GetResponse<GetRideDatesResult>(new
            {
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            });

            var rides = result.Message.Rides;

            var groupedRides = GroupBy(request.GroupBy, request.DateFrom, request.DateTo, rides);

            var builder = new StringBuilder();

            builder.AppendLine($"{ CreateChartTitle(request.GroupBy) };Rides number");

            foreach (var ride in groupedRides)
            {
                builder.AppendLine($"{ride.Item1};{ride.Item2}");
            }

            return builder.ToString();
        }

        public async Task<string> GetJsonDataStatistics(GetStatisticsRequest request)
        {
            var result = await _requestClient.GetResponse<GetRideDatesResult>(new
            {
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            });

            var rides = result.Message.Rides;

            var groupedRides = GroupBy(request.GroupBy, request.DateFrom, request.DateTo, rides);
            var data = groupedRides.Select(c => new JsonStatsElement
            {
                Label = c.Item1,
                Value = c.Item2
            });

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(new JsonStatsDTO
            {
                Data = data
            }, options);
        }

        public async Task<IResultResponse<ChartStatsDTO>> GetChartStatistics(GetStatisticsRequest request)
        {
            var response = new ResultResponse<ChartStatsDTO>();

            var result = await _requestClient.GetResponse<GetRideDatesResult>(new
            {
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            });

            var rides = result.Message.Rides;

            var groupedRides = GroupBy(request.GroupBy, request.DateFrom, request.DateTo, rides);

            response.Result = new ChartStatsDTO
            {
                XTitle = $"Rides Per { CreateChartTitle(request.GroupBy) }",
                YTitle = "Rides number",
                XValues = groupedRides.Select(c => c.Item1),
                YValues = groupedRides.Select(c => c.Item2)
            };

            return response;
        }

        private IEnumerable<(string, int)> GroupBy(GroupByType groupBy, DateTimeOffset start, DateTimeOffset end, IEnumerable<DateTimeOffset> values)
        {
            var result = new List<(string, int)>();

            for (DateTimeOffset i = start.Date; i < end.Date; i = SetIntervalByGroupBy(i, groupBy))
            {
                var startInterval = i;
                var endInterval = SetIntervalByGroupBy(i, groupBy);

                var label = $"{startInterval.ToString(GetFormatByGroupBy(groupBy))}";
                result.Add((label, values.Count(c => c >= startInterval && c < endInterval)));
            }

            return result;
        }

        private DateTimeOffset SetIntervalByGroupBy(DateTimeOffset dateTime, GroupByType groupBy)
        {
            return groupBy switch
            {
                GroupByType.Day => dateTime.AddDays(1),
                GroupByType.Week => dateTime.AddDays(7),
                GroupByType.Month => dateTime.AddMonths(1),
                GroupByType.Year => dateTime.AddYears(1),
                _ => dateTime.AddDays(1),
            };
        }

        private string GetFormatByGroupBy(GroupByType groupBy)
        {
            return groupBy switch
            {
                GroupByType.Day => DayFormat,
                GroupByType.Week => WeekFormat,
                GroupByType.Month => MonthFormat,
                GroupByType.Year => YearFormat,
                _ => DayFormat
            };
        }

        private string CreateChartTitle(GroupByType groupBy)
        {
            return groupBy switch
            {
                GroupByType.Day => "Day",
                GroupByType.Week => "Week",
                GroupByType.Month => "Month",
                GroupByType.Year => "Year",
                _ => "Day"
            };
        }
    }
}