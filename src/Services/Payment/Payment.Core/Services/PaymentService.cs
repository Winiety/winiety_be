using Contracts.Events;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using Payment.Core.Interfaces;
using Payment.Core.Model.DTOs;
using Payment.Core.Model.Requests;
using Shared.Core.BaseModels.Responses;
using System.Linq;
using PayU.Client.Configurations;
using PayU.Client;
using System.Collections.Generic;
using PayU.Client.Models;
using System.Threading;
using Microsoft.Extensions.Options;
using Payment.Core.Options;
using System;
using Shared.Core.BaseModels.Requests;

namespace Payment.Core.Services
{
    public interface IPaymentService
    {
        Task CreatePaymentAsync(RideRegistered rideEvent);
        Task<IPagedResponse<PaymentDetailDTO>> GetPaymentsAsync(PaymentSearchRequest search);
        Task<IPagedResponse<PaymentDTO>> GetUserPaymentsAsync(PaymentSearchRequest search);
        Task<IPagedResponse<WinietaDTO>> GetUserWinietasAsync(SearchRequest search);
        Task<ResultResponse<PaymentDTO>> PayAsync(PayRequest pay);
        Task<BaseResponse> NotifyAsync(NotifyRequest notify);
        Task<ResultResponse<WinietaDTO>> BuyWinietaAsync(string continueUrl);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IWinietaRepository _winietaRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ILogger<PaymentService> _logger;
        private readonly PayuOptions _payuOptions;

        public PaymentService(
            IWinietaRepository winietaRepository,
            IPaymentRepository paymentRepository,
            IMapper mapper,
            IUserContext userContext,
            IOptions<PayuOptions> payuOptions,
            ILogger<PaymentService> logger)
        {
            _winietaRepository = winietaRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _userContext = userContext;
            _payuOptions = payuOptions.Value;
            _logger = logger;
        }

        public async Task<ResultResponse<WinietaDTO>> BuyWinietaAsync(string continueUrl)
        {
            var response = new ResultResponse<WinietaDTO>();

            var userId = _userContext.GetUserId();
            var settings = new PayUClientSettings(_payuOptions.PayUApiUrl, "v2_1", _payuOptions.ClientId, _payuOptions.ClientSecret);
            var client = new PayUClient(settings);

            var request = new OrderRequest("127.0.0.1", settings.ClientId, "Winiety", "PLN", "20000", new List<Product> { new Product($"Winieta-{userId}-{DateTime.UtcNow.ToString("s")}", "20000", "1") });

            request.ContinueUrl = continueUrl;
            request.NotifyUrl = _payuOptions.NotifyUrl + "/api/Payment";

            var result = await client.PostOrderAsync(request, default(CancellationToken));
            var winieta = new Model.Entities.Winieta
            {
                UserId = userId,
                PaymentStatus = "NEW",
                Cost = 200,
                ExpirationDate = DateTime.UtcNow.AddMonths(1),
                OrderId = result.OrderId,
                PayuUrl = result.RedirectUri
            };

            await _winietaRepository.AddAsync(winieta);

            return _mapper.Map(winieta, response);
        }

        public async Task CreatePaymentAsync(RideRegistered rideEvent)
        {
            var winietas = await _winietaRepository.GetAllByAsync(c => c.UserId == rideEvent.UserId && c.ExpirationDate > DateTime.UtcNow);

            if (!winietas.Any())
            {
                var payment = new Model.Entities.Payment
                {
                    RideId = rideEvent.Id,
                    UserId = rideEvent.UserId,
                    Status = "NEW",
                    Amount = 10
                };

                await _paymentRepository.AddAsync(payment);
            }
        }

        public async Task<IPagedResponse<PaymentDetailDTO>> GetPaymentsAsync(PaymentSearchRequest search)
        {
            var response = new PagedResponse<PaymentDetailDTO>();

            var paymentsQuery = _paymentRepository.GetQueryable();
            paymentsQuery = CreateSearchQuery(paymentsQuery, search, false);

            var payments = await _paymentRepository.GetPagedByAsync(
                 paymentsQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(payments, response);

            return response;
        }

        public async Task<IPagedResponse<PaymentDTO>> GetUserPaymentsAsync(PaymentSearchRequest search)
        {
            var response = new PagedResponse<PaymentDTO>();

            var paymentsQuery = _paymentRepository.GetQueryable();
            paymentsQuery = CreateSearchQuery(paymentsQuery, search, true);
            paymentsQuery = paymentsQuery.OrderByDescending(c => c.Id);

            var payments = await _paymentRepository.GetPagedByAsync(
                 paymentsQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(payments, response);

            return response;
        }

        public async Task<IPagedResponse<WinietaDTO>> GetUserWinietasAsync(SearchRequest search)
        {
            var response = new PagedResponse<WinietaDTO>();

            var winietasQuery = _winietaRepository.GetQueryable();
            winietasQuery = CreateWinietasSearchQuery(winietasQuery, true);
            winietasQuery = winietasQuery.OrderByDescending(c => c.Id);

            var winietas = await _winietaRepository.GetPagedByAsync(
                 winietasQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(winietas, response);

            return response;
        }

        public async Task<BaseResponse> NotifyAsync(NotifyRequest notify)
        {
            var response = new BaseResponse();

            var payment = await _paymentRepository.GetByAsync(p => p.OrderId.Equals(notify.order.OrderId));
            var winieta = await _winietaRepository.GetByAsync(p => p.OrderId.Equals(notify.order.OrderId));

            if (payment is null && winieta is null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono płatności."
                });

                return response;
            }

            if (payment != null)
            {
                payment.Status = notify.order.Status;
                await _paymentRepository.UpdateAsync(payment);
            }

            if (winieta != null)
            {
                winieta.PaymentStatus = notify.order.Status;
                await _winietaRepository.UpdateAsync(winieta);
            }

            return response;
        }

        public async Task<ResultResponse<PaymentDTO>> PayAsync(PayRequest pay)
        {
            var response = new ResultResponse<PaymentDTO>();

            var payment = await _paymentRepository.GetAsync(pay.PaymentId);

            if (payment is null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono płatności."
                });

                return response;
            }

            if (payment.PayuUrl is null || payment.Status.Equals("CANCELED"))
            {
                var settings = new PayUClientSettings(_payuOptions.PayUApiUrl, "v2_1", _payuOptions.ClientId, _payuOptions.ClientSecret);
                PayUClient client = new PayUClient(settings);

                var request = new OrderRequest("127.0.0.1", settings.ClientId, "Winiety", "PLN", (payment.Amount * 100).ToString(), new List<Product> { new Product(payment.RideId.ToString(), (payment.Amount * 100).ToString(), "1") });
                request.ContinueUrl = pay.ContinueUrl;
                request.NotifyUrl = _payuOptions.NotifyUrl + "/api/Payment";

                var result = await client.PostOrderAsync(request, default(CancellationToken));

                payment.Status = "NEW";
                payment.OrderId = result.OrderId;
                payment.PayuUrl = result.RedirectUri;
                await _paymentRepository.UpdateAsync(payment);
            }

            return _mapper.Map(payment, response);
        }

        private IQueryable<Model.Entities.Payment> CreateSearchQuery(IQueryable<Model.Entities.Payment> query, PaymentSearchRequest search, bool isUser)
        {
            if (!string.IsNullOrWhiteSpace(search.Status))
            {
                query = query.Where(c => c.Status.Equals(search.Status));
            }

            if (isUser)
            {
                var currentUserId = _userContext.GetUserId();
                query = query.Where(c => c.UserId == currentUserId);
            }

            return query;
        }

        private IQueryable<Model.Entities.Winieta> CreateWinietasSearchQuery(IQueryable<Model.Entities.Winieta> query, bool isUser)
        {
            if (isUser)
            {
                var currentUserId = _userContext.GetUserId();
                query = query.Where(c => c.UserId == currentUserId);
            }

            return query;
        }
    }
}
