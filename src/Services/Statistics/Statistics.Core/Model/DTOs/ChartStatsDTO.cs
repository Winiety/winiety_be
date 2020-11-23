using Shared.Core.BaseModels.Responses.Interfaces;
using System.Collections.Generic;

namespace Statistics.Core.Model.DTOs
{
    public class ChartStatsDTO : IResponseDTO
    {
        public string XTitle { get; set; }
        public string YTitle { get; set; }
        public IEnumerable<string> XValues { get; set; }
        public IEnumerable<int> YValues { get; set; }
    }
}
