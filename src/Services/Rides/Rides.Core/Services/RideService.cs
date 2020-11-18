using AutoMapper;
using Contracts.Commands;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Rides.Core.Interfaces;
using Rides.Core.Model.DTOs;
using Rides.Core.Model.Entities;
using Rides.Core.Model.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rides.Core.Services
{
    public interface IRideService
    {
        Task RegisterRideAsync(int pictureId, string plateNumber);
        Task<IPagedResponse<RideDetailDTO>> GetRidesAsync(RideSearchRequest search);
        Task<IPagedResponse<RideDTO>> GetUserRidesAsync(RideSearchRequest search);

    }

    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IBusControl _bus;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RideService> _logger;

        public RideService(IRideRepository rideRepository, IBusControl bus, IUserContext userContext, IMapper mapper, ILogger<RideService> logger)
        {
            _rideRepository = rideRepository;
            _bus = bus;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IPagedResponse<RideDetailDTO>> GetRidesAsync(RideSearchRequest search)
        {
            var response = new PagedResponse<RideDetailDTO>();

            var ridesQuery = _rideRepository.GetQueryable();
            ridesQuery = CreateSearchQuery(ridesQuery, search, false);

            var rides = await _rideRepository.GetPagedByAsync(
                 ridesQuery,
                 search.PageNumber,
                 search.PageSize);

            _mapper.Map(rides, response);

            return response;
        }

        public async Task<IPagedResponse<RideDTO>> GetUserRidesAsync(RideSearchRequest search)
        {
            var response = new PagedResponse<RideDTO>();

            var ridesQuery = _rideRepository.GetQueryable();
            ridesQuery = CreateSearchQuery(ridesQuery, search, true);

            var rides = await _rideRepository.GetPagedByAsync(
                 ridesQuery,
                 search.PageNumber,
                 search.PageSize);

            _mapper.Map(rides, response);

            return response;
        }

        public async Task RegisterRideAsync(int pictureId, string plateNumber)
        {
            var uri = new Uri("rabbitmq://localhost/profile-listener");

            var requestClient = _bus.CreateRequestClient<GetUserIdByPlate>(uri);

            var response = await requestClient.GetResponse<GetUserIdByPlateResult>(new
            {
                PlateNumber = plateNumber
            });

            var ride = new Ride
            {
                PictureId = pictureId,
                PlateNumber = plateNumber,
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = response.Message.UserId
            };

            await _rideRepository.AddAsync(ride);

            _logger.LogInformation($"Ride registered - [ID={ride.Id}] [PlateNumber={ride.PlateNumber}] [PictureId={ride.PictureId}] [UserId={ride.UserId}] [RideDateTime={ride.RideDateTime}]");
        }

        private IQueryable<Ride> CreateSearchQuery(IQueryable<Ride> query, RideSearchRequest search, bool isUserRides)
        {
            if (isUserRides)
            {
                var currentUserId = _userContext.GetUserId();
                query = query.Where(c => c.UserId == currentUserId);
            }

            if (search.StartDate.HasValue)
            {
                query = query.Where(c => c.RideDateTime >= search.StartDate);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(c => c.RideDateTime <= search.EndDate);
            }

            var searchQuery = search.Query;

            return query.Where(c => c.PlateNumber.Contains(searchQuery));
        }
    }
}
