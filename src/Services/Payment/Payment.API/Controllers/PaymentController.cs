using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Model.DTOs;
using Payment.Core.Model.Requests;
using Payment.Core.Services;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IPagedResponse<PaymentDetailDTO>>> GetPayments([FromQuery] PaymentSearchRequest searchRequest)
        {
            var response = await _paymentService.GetPaymentsAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IPagedResponse<PaymentDTO>>> GetUserPayments([FromQuery] PaymentSearchRequest searchRequest)
        {
            var response = await _paymentService.GetUserPaymentsAsync(searchRequest);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("pay")]
        [Authorize]
        public async Task<ActionResult<ResultResponse<PaymentDTO>>> Pay([FromBody] PayRequest pay)
        {
            var response = await _paymentService.PayAsync(pay);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Notify([FromBody] NotifyRequest notify)
        {
            var response = await _paymentService.NotifyAsync(notify);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
