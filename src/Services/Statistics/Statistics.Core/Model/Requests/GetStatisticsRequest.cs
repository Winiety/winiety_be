using System;

namespace Statistics.Core.Model.Requests
{
    public class GetStatisticsRequest
    {
        public DateTimeOffset DateFrom { get; set; } = DateTimeOffset.UtcNow.AddDays(-30).Date;
        public DateTimeOffset DateTo { get; set; } = DateTimeOffset.UtcNow.AddDays(1).Date;
        public GroupByType GroupBy { get; set; } = GroupByType.Day;
    }
}
