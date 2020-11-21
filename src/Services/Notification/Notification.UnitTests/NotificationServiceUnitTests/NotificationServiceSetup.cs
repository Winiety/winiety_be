using AutoMapper;
using Contracts.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Core.Common;
using Notification.Core.Hubs;
using Notification.Core.Interfaces;
using Notification.Core.Model.DTOs;
using Notification.Core.Model.Entities;
using Notification.Core.Services;
using Shared.Core.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notification.UnitTests.NotificationServiceUnitTests
{
    public class NotificationServiceSetup
    {
        protected readonly Mock<INotificationRepository> _notificationRepository;
        protected readonly Mock<IMapper> _mapper;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IHubContext<NotificationHub>> _notificationContext;
        protected readonly Mock<IHubClients> _hubClients;
        protected readonly Mock<IClientProxy> _clientProxy;
        protected readonly Mock<ILogger<NotificationService>> _logger;
        protected readonly NotificationService _notificationService;

        public NotificationServiceSetup()
        {
            _notificationRepository = new Mock<INotificationRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<NotificationService>>();
            _userContext = new Mock<IUserContext>();
            _notificationContext = new Mock<IHubContext<NotificationHub>>();
            _notificationService = new NotificationService(_notificationRepository.Object, _mapper.Object, _userContext.Object, _notificationContext.Object, _logger.Object);

            _hubClients = new Mock<IHubClients>();
            _clientProxy = new Mock<IClientProxy>();
        }

        public IEnumerable<NotificationModel> GetNotifications()
        {
            return new List<NotificationModel>
            {
                new NotificationModel
                {
                    Id = 1,
                    Content = "content",
                    IsRead = false,
                    DateTime = DateTimeOffset.UtcNow,
                    NotificationType = NotificationTypes.Ride,
                    RedirectId = 1,
                    UserId = 1
                },
                new NotificationModel
                {
                    Id = 2,
                    Content = "content",
                    IsRead = false,
                    DateTime = DateTimeOffset.UtcNow,
                    NotificationType = NotificationTypes.Ride,
                    RedirectId = 2,
                    UserId = 2
                },
                new NotificationModel
                {
                    Id = 3,
                    Content = "content",
                    IsRead = false,
                    DateTime = DateTimeOffset.UtcNow,
                    NotificationType = NotificationTypes.Ride,
                    RedirectId = 3,
                    UserId = 3
                },
                new NotificationModel
                {
                    Id = 4,
                    Content = "content",
                    IsRead = false,
                    DateTime = DateTimeOffset.UtcNow,
                    NotificationType = NotificationTypes.Ride,
                    RedirectId = 4,
                    UserId = 4
                }
            };
        }

        public IEnumerable<NotificationDTO> GetNotificationsDTO(IEnumerable<NotificationModel> notifications)
        {
            return notifications.Select(c => new NotificationDTO
            {
                Id = c.Id,
                Content = c.Content,
                IsRead = c.IsRead,
                RedirectId = c.RedirectId,
                NotificationType = c.NotificationType,
                DateTime = c.DateTime
            });
        }

        public class PagedList<T> : List<T>, IPagedList<T>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
        }

        public class RideRegisteredMock : RideRegistered
        {
            public int Id { get; set; }
            public int? UserId { get; set; }
            public string PlateNumber { get; set; }
            public DateTimeOffset RideDateTime { get; set; }
        }
    }
}
