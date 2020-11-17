using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rides.Core.Model.DTOs;
using Rides.Core.Services;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<IPagedResponse<RideDTO>>> GetRides(SearchRequest searchRequest)
        {
            var response = await _rideService.GetRidesAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IPagedResponse<RideDTO>>> GetUserRides(SearchRequest searchRequest)
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
