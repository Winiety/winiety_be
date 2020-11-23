using System.Collections.Generic;

namespace Statistics.Core.Model.DTOs
{
    public class JsonStatsDTO
    {
        public IEnumerable<JsonStatsElement> Data { get; set; }
    }

    public class JsonStatsElement
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
