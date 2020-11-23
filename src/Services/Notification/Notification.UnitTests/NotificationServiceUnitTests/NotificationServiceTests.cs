using Moq;
using Notification.Core.Model.DTOs;
using Notification.Core.Model.Entities;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Notification.UnitTests.NotificationServiceUnitTests
{
    public class NotificationServiceTests : NotificationServiceSetup
    {
        [Fact]
        public async Task GetNotificationsAsync_Should_ReturnNotifications()
        {
            var notifications = GetNotifications();
            var searchRequest = new SearchRequest();
            var paged = new PagedList<NotificationModel>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = notifications.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<NotificationDTO>()
            {
                TotalPages = paged.TotalPages,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = paged.TotalCount,
                PageSize = searchRequest.PageSize,
                Results = GetNotificationsDTO(notifications)
            };

            paged.AddRange(notifications);

            _notificationRepository
                .Setup(c => c.GetQueryable())
                .Returns(notifications.AsQueryable());

            _notificationRepository
                .Setup(c => c.GetPagedByAsync(
                    It.IsAny<Expression<Func<NotificationModel, bool>>>(),
                    It.IsAny<Expression<Func<NotificationModel, DateTimeOffset>>>(),
                    searchRequest.PageNumber,
                    searchRequest.PageSize,
                    false,
                    true))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<NotificationModel>>(), It.IsAny<PagedResponse<NotificationDTO>>()))
                .Returns(response);

            var result = await _notificationService.GetNotificationsAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(notifications.Count(), result.TotalCount);
        }

        [Fact]
        public async Task CreateNotificationAsync_Should_AddNotification()
        {
            var request = new RideRegisteredMock
            {
                Id = 1,
                PlateNumber = "112233",
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = 1
            };

            _notificationRepository
              .Setup(c => c.AddAsync(It.IsAny<NotificationModel>()));

            _notificationContext
                .Setup(c => c.Clients)
                .Returns(_hubClients.Object);

            _hubClients
                .Setup(c => c.User(It.IsAny<string>()))
                .Returns(_clientProxy.Object);

            await _notificationService.CreateNotificationAsync(request);

            _notificationRepository.Verify(c => c.AddAsync(It.IsAny<NotificationModel>()), Times.Once());
            _mapper.Verify(c => c.Map<NotificationDTO>(It.IsAny<NotificationModel>()), Times.Once());
        }
    }
}
