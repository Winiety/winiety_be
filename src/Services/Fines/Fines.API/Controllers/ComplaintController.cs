using Fines.Core.Model.DTOs.Complaint;
using Fines.Core.Model.Requests.Complaint;
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
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IResultResponse<ComplaintDetailDTO>>> GetComplaint(int id)
        {
            var response = await _complaintService.GetComplaintAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResponse<ComplaintDetailDTO>>> GetComplaints([FromQuery] SearchRequest searchRequest)
        {
            var response = await _complaintService.GetComplaintsAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IPagedResponse<ComplaintDTO>>> GetUserComplaints([FromQuery] SearchRequest searchRequest)
        {
            var response = await _complaintService.GetUserComplaintsAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<IResultResponse<ComplaintDTO>>> CreateComplaint([FromBody] CreateComplaintRequest complaint)
        {
            var response = await _complaintService.CreateComplaintAsync(complaint);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<IResultResponse<ComplaintDTO>>> UpdateComplaint([FromBody] UpdateComplaintRequest complaint)
        {
            var response = await _complaintService.UpdateComplaintAsync(complaint);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IBaseResponse>> DeleteComplaint(int id)
        {
            var response = await _complaintService.RemoveComplaintAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
