using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pictures.Application.Services;

namespace Pictures.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _pictureService;

        public PicturesController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPicture(IFormFile file)
        {
            using var ms = new MemoryStream();
            
            file.CopyTo(ms);

            var fileBytes = ms.ToArray();

            var response = await _pictureService.AddPictureAsync(fileBytes);

            return Ok(response);
        }
    }
}
