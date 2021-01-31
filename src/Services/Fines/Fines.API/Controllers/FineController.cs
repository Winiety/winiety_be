using Fines.Core.Model.DTOs.Fine;
using Fines.Core.Model.Requests.Fine;
using Fines.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;
using System.Threading.Tasks;

namespace Fines.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FineController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IResultResponse<FineDetailDTO>>> GetFine(int id)
        {
            var response = await _fineService.GetFineAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "police, admin")]
        public async Task<ActionResult<IPagedResponse<FineDetailDTO>>> GetFines([FromQuery] SearchRequest searchRequest)
        {
            var response = await _fineService.GetFinesAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IPagedResponse<FineDTO>>> GetUserFines([FromQuery] SearchRequest searchRequest)
        {
            var response = await _fineService.GetUserFinesAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "police, admin")]
        public async Task<ActionResult<IResultResponse<FineDTO>>> CreateFine([FromBody] CreateFineRequest fine)
        {
            var response = await _fineService.CreateFineAsync(fine);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "police, admin")]
        public async Task<ActionResult<IResultResponse<FineDTO>>> UpdateFine([FromBody] UpdateFineRequest fine)
        {
            var response = await _fineService.UpdateFineAsync(fine);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "police, admin")]
        public async Task<ActionResult<IBaseResponse>> DeleteFine(int id)
        {
            var response = await _fineService.RemoveFineAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
