using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Rides.Core.Interfaces;
using Rides.Core.Model;
using Rides.Core.Model.DTOs;
using Rides.Core.Model.Entities;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rides.Core.Services
{
    public interface IRideService
    {
        Task RegisterRideAsync(CarRegistered car);
        Task<IPagedResponse<RideDTO>> GetRidesAsync(SearchRequest searchReques);
        Task<IPagedResponse<RideDTO>> GetUserRidesAsync(SearchRequest searchReques);
    }

    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IBusControl _bus;
        private readonly ILogger<RideService> _logger;

        public RideService(IRideRepository rideRepository, IBusControl bus, IUserContext userContext, IMapper mapper, ILogger<RideService> logger)
        {
            _rideRepository = rideRepository;
            _bus = bus;
            _logger = logger;
        }

        public Task<IPagedResponse<RideDTO>> GetRidesAsync(SearchRequest searchReques)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedResponse<RideDTO>> GetUserRidesAsync(SearchRequest searchReques)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterRideAsync(CarRegistered car)
        {
            var uri = new Uri("rabbitmq://localhost/profile-listener");

            var requestClient = _bus.CreateRequestClient<GetUserIdByPlate>(uri);

            var response = await requestClient.GetResponse<GetUserIdByPlateResult>(new
            {
                PlateNumber = car.PlateNumber
            });

            var ride = new Ride
            {
                PictureId = car.PictureId,
                PlateNumber = car.PlateNumber,
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = response.Message.UserId
            };

            await _rideRepository.AddAsync(ride);

            _logger.LogInformation($"Ride registered - [ID={ride.Id}] [PlateNumber={ride.PlateNumber}] [PictureId={ride.PictureId}] [UserId={ride.UserId}] [RideDateTime={ride.RideDateTime}]");
        }

        private Expression<Func<Ride, bool>> CreateSearchExpression(SearchRequest search)
        {
            var currentUserId = _userContext.GetUserId();
            var query = search.Query;

            return c => c.UserId == currentUserId &&
                (c.PlateNumber.Contains(query) ||
                 c.Model.Contains(query) ||
                 c.Brand.Contains(query) ||
                 c.Color.Contains(query) ||
                 c.Year.Contains(query));
        }
    }
}
