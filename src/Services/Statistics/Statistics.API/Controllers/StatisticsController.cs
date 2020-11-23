using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.BaseModels.Responses.Interfaces;
using Statistics.Core.Model.DTOs;
using Statistics.Core.Model.Requests;
using Statistics.Core.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("chart")]
        public async Task<ActionResult<IResultResponse<ChartStatsDTO>>> GetChartsStatistics([FromQuery] GetStatisticsRequest searchRequest)
        {
            var response = await _statisticsService.GetChartStatistics(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("csv")]
        public async Task<FileContentResult> GetCSVStatistics([FromQuery] GetStatisticsRequest searchRequest)
        {
            var response = await _statisticsService.GetCsvDataStatistics(searchRequest);

            var result = Encoding.ASCII.GetBytes(response);

            return File(result, "text/csv", $"stats_{DateTimeOffset.Now.ToString("MM_dd_yyyy")}.csv");
        }

        [HttpGet("json")]
        public async Task<FileContentResult> GetJsonStatistics([FromQuery] GetStatisticsRequest searchRequest)
        {
            var response = await _statisticsService.GetJsonDataStatistics(searchRequest);

            var result = Encoding.ASCII.GetBytes(response);

            return File(result, "text/json", $"stats_{DateTimeOffset.Now.ToString("MM_dd_yyyy")}.json");
        }
    }
}
