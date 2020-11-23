using AutoMapper;
using Contracts.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Core.Common;
using Notification.Core.Hubs;
using Notification.Core.Interfaces;
using Notification.Core.Model.DTOs;
using Notification.Core.Model.Entities;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Threading.Tasks;

namespace Notification.Core.Services
{
    public interface INotificationService
    {
        Task<IPagedResponse<NotificationDTO>> GetNotificationsAsync(SearchRequest search);
        Task CreateNotificationAsync(RideRegistered rideEvent);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IMapper mapper,
            IUserContext userContext,
            IHubContext<NotificationHub> notificationContext,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _userContext = userContext;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task CreateNotificationAsync(RideRegistered rideEvent)
        {
            var notification = new NotificationModel
            {
                UserId = rideEvent.UserId.Value,
                Content = $"Registered ride with license plates - [{rideEvent.PlateNumber}]. ({rideEvent.RideDateTime})",
                IsRead = false,
                NotificationType = NotificationTypes.Ride,
                RedirectId = rideEvent.Id,
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

        private async Task SendNotification(int userId, NotificationDTO notification)
        {
            await _notificationContext.Clients.User(userId.ToString()).SendAsync("newNotification", notification);
        }
    }
}
