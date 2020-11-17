using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Models.DTOs.UserProfile;
using Profile.Core.Models.Requests.UserProfile;
using Profile.Core.Services;
using Shared.Core.BaseModels.Responses.Interfaces;
using System.Threading.Tasks;

namespace Profile.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public ProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<ActionResult<IResultResponse<UserProfileDTO>>> GetProfile()
        {
            var response = await _userProfileService.GetUserProfileAsync();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<IResultResponse<UserProfileDTO>>> UpdateCar([FromBody] UpdateUserProfileRequest userProfile)
        {
            var response = await _userProfileService.UpdateProfileAsync(userProfile);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
