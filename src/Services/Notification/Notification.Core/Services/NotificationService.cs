using AutoMapper;
using Contracts.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Core.Common;
using Notification.Core.Hubs;
using Notification.Core.Interfaces;
using Notification.Core.Model.DTOs;
using Notification.Core.Model.Entities;
using Notification.Core.Model.Requests;
using Notification.Core.Options;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebPush;

namespace Notification.Core.Services
{
    public interface INotificationService
    {
        Task<IPagedResponse<NotificationDTO>> GetNotificationsAsync(SearchRequest search);
        Task CreateNotificationAsync(RideRegistered rideEvent);
        Task CreateNotificationAsync(ComplaintRegistered complaintEvent);
        Task CreateNotificationAsync(FineRegistered fineEvent);
        Task RegisterSubscriptionAsync(RegisterSubscriptionRequest subscriptionRequest);
        IResultResponse<PublicKeyResponse> GetPublicKey();
    }

    public class NotificationService : INotificationService
    {
        private const string DateTimeFormat = "HH:mm dd-MM-yyyy";

        private readonly INotificationRepository _notificationRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly WebPushOptions _webPushOptions;
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            ISubscriptionRepository subscriptionRepository,
            IMapper mapper,
            IUserContext userContext,
            IOptions<WebPushOptions> webPushOptions,
            IHubContext<NotificationHub> notificationContext,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _userContext = userContext;
            _webPushOptions = webPushOptions.Value;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task CreateNotificationAsync(RideRegistered rideEvent)
        {
            var notification = new NotificationModel
            {
                UserId = rideEvent.UserId.Value,
                Content = $"Przejazd zarejestrowany na tablice rejestracyjne - [{rideEvent.PlateNumber}]. ({rideEvent.RideDateTime.ToString(DateTimeFormat)})",
                IsRead = false,
                NotificationType = NotificationTypes.Ride,
                RedirectId = rideEvent.Id,
                DateTime = DateTimeOffset.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            var notificationDTO = _mapper.Map<NotificationDTO>(notification);

            await SendNotification(notification.UserId, notificationDTO);
        }

        public async Task CreateNotificationAsync(ComplaintRegistered complaintEvent)
        {
            var notification = new NotificationModel
            {
                UserId = complaintEvent.UserId,
                Content = $"Skarga do przejazdu [nr. {complaintEvent.RideId}] została zgłoszona. Opis [{TakeNLetters(complaintEvent.Description, 40)}...]. ({complaintEvent.CreateDateTime.ToString(DateTimeFormat)})",
                IsRead = false,
                NotificationType = NotificationTypes.Complaint,
                RedirectId = complaintEvent.Id,
                DateTime = DateTimeOffset.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            var notificationDTO = _mapper.Map<NotificationDTO>(notification);

            await SendNotification(notification.UserId, notificationDTO);
        }

        public async Task CreateNotificationAsync(FineRegistered fineEvent)
        {
            var notification = new NotificationModel
            {
                UserId = fineEvent.UserId,
                Content = $"Mandat za przejazd [nr. {fineEvent.RideId}]. Wysokość mandatu [{fineEvent.Cost} PLN]. Opis [{TakeNLetters(fineEvent.Description, 40)}...]. ({fineEvent.CreateDateTime.ToString(DateTimeFormat)})",
                IsRead = false,
                NotificationType = NotificationTypes.Fine,
                RedirectId = fineEvent.Id,
                DateTime = DateTimeOffset.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            var notificationDTO = _mapper.Map<NotificationDTO>(notification);

            await SendNotification(notification.UserId, notificationDTO);
        }

        public async Task<IPagedResponse<NotificationDTO>> GetNotificationsAsync(SearchRequest search)
        {
            var response = new PagedResponse<NotificationDTO>();

            var currentUserId = _userContext.GetUserId();

            var pictures = await _notificationRepository.GetPagedByAsync(
                c => c.UserId == currentUserId,
                o => o.DateTime,
                search.PageNumber,
                search.PageSize);

            response = _mapper.Map(pictures, response);

            return response;
        }

        public IResultResponse<PublicKeyResponse> GetPublicKey()
        {
            var response = new ResultResponse<PublicKeyResponse>
            {
                Result = new PublicKeyResponse
                {
                    PublicKey = _webPushOptions.PublicKey
                }
            };

            return response;
        }

        public async Task RegisterSubscriptionAsync(RegisterSubscriptionRequest subscriptionRequest)
        {
            var currentUserId = _userContext.GetUserId();

            var subscription = new SubscriptionModel
            {
                Auth = subscriptionRequest.Auth,
                P256dh = subscriptionRequest.P256dh,
                Endpoint = subscriptionRequest.Endpoint,
                UserId = currentUserId
            };

            await _subscriptionRepository.AddAsync(subscription);
        }

        private async Task SendNotification(int userId, NotificationDTO notification)
        {
            var subscriptions = await _subscriptionRepository.GetAllByAsync(c => c.UserId == userId);
            if (subscriptions == null || subscriptions.Count() == 0)
            {
                return;
            }

            var subject = _webPushOptions.Subject;
            var publicKey = _webPushOptions.PublicKey;
            var privateKey = _webPushOptions.PrivateKey;

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

            var webPushClient = new WebPushClient();
            try
            {
                foreach (var subscription in subscriptions)
                {
                    var sub = new PushSubscription
                    {
                        Auth = subscription.Auth,
                        Endpoint = subscription.Endpoint,
                        P256DH = subscription.P256dh
                    };
                    var message = JsonSerializer.Serialize(notification);
                    webPushClient.SendNotification(sub, message, vapidDetails);
                }
            }
            catch (Exception)
            {
            }

            await _notificationContext.Clients.User(userId.ToString()).SendAsync("newNotification", notification);
        }

        private string TakeNLetters(string text, int n)
        {
            var length = text.Length;
            if (n > length)
            {
                n = length;
            }

            return text.Substring(0, n);
        }
    }
}
