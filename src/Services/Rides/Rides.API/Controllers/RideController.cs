using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rides.Core.Model.DTOs;
using Rides.Core.Model.Requests;
using Rides.Core.Services;
using Shared.Core.BaseModels.Responses.Interfaces;
using System.Threading.Tasks;

namespace Rides.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpGet]
        [Authorize(Roles = "police, admin")]
        public async Task<ActionResult<IPagedResponse<RideDetailDTO>>> GetRides([FromQuery] RideSearchRequest searchRequest)
        {
            var response = await _rideService.GetRidesAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IPagedResponse<RideDTO>>> GetUserRides([FromQuery] RideSearchRequest searchRequest)
        {
            var response = await _rideService.GetUserRidesAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
