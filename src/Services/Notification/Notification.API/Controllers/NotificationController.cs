using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Core.Model.DTOs;
using Notification.Core.Model.Requests;
using Notification.Core.Services;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;
using System.Threading.Tasks;

namespace Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResponse<NotificationDTO>>> GetNotifications([FromQuery] SearchRequest searchRequest)
        {
            var response = await _notificationService.GetNotificationsAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSubscriptionRequest subscriptionRequest)
        {
            await _notificationService.RegisterSubscriptionAsync(subscriptionRequest);

            return Ok();
        }

        [HttpGet("key")]
        public IActionResult GetPublicKey()
        {
            var response = _notificationService.GetPublicKey();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
