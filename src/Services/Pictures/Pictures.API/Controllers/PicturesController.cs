using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pictures.Core.Model.DTOs;
using Pictures.Core.Model.Requests;
using Pictures.Core.Services;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;

namespace Pictures.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public PicturesController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddPicture([FromForm] AddPictureRequest request)
        {
            var response = await _pictureService.AddPictureAsync(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PictureDTO>> GetPicture(int id)
        {
            var response = await _pictureService.GetPictureAsync(id);

            return Ok(response);
        }

        [HttpGet("not-recognized")]
        [Authorize(Roles = "corrector, admin")]
        public async Task<ActionResult<IPagedResponse<PictureDTO>>> GetNotRecognizedPicturse([FromQuery] SearchRequest search)
        {
            var response = await _pictureService.GetNotRecognizedPicturesAsync(search);

            return Ok(response);
        }
    }
}
