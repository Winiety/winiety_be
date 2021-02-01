using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rides.Core.Services
{
    public interface IRideService
    {
        Task RegisterRideAsync(int pictureId, string plateNumber, double speed, DateTimeOffset rideDateTime);
        Task<IPagedResponse<RideDetailDTO>> GetRidesAsync(RideSearchRequest search);
        Task<IPagedResponse<RideDTO>> GetUserRidesAsync(RideSearchRequest search);
        Task<IEnumerable<DateTimeOffset>> GetRidesForStatisticsAsync(DateTimeOffset startDate, DateTimeOffset endDate);
        Task<RideDetailDTO> GetRideForFinesAsync(int rideId);
    }

    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IBusControl _bus;
        private readonly IRequestClient<GetUserIdByPlate> _requestClient;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RideService> _logger;

        public RideService(
            IRideRepository rideRepository,
            IBusControl bus,
            IRequestClient<GetUserIdByPlate> requestClient,
            IUserContext userContext,
            IMapper mapper,
            ILogger<RideService> logger)
        {
            _rideRepository = rideRepository;
            _bus = bus;
            _requestClient = requestClient;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IPagedResponse<RideDetailDTO>> GetRidesAsync(RideSearchRequest search)
        {
            var response = new PagedResponse<RideDetailDTO>();

            var ridesQuery = _rideRepository.GetQueryable();
            ridesQuery = CreateSearchQuery(ridesQuery, search, false);
            ridesQuery.OrderByDescending(c => c.RideDateTime);

            var rides = await _rideRepository.GetPagedByAsync(
                 ridesQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(rides, response);

            return response;
        }

        public async Task<IPagedResponse<RideDTO>> GetUserRidesAsync(RideSearchRequest search)
        {
            var response = new PagedResponse<RideDTO>();

            var ridesQuery = _rideRepository.GetQueryable();
            ridesQuery = CreateSearchQuery(ridesQuery, search, true);
            ridesQuery.OrderByDescending(c => c.RideDateTime);

            var rides = await _rideRepository.GetPagedByAsync(
                 ridesQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(rides, response);

            return response;
        }

        public async Task RegisterRideAsync(int pictureId, string plateNumber, double speed, DateTimeOffset rideDateTime)
        {
            var response = await _requestClient.GetResponse<GetUserIdByPlateResult>(new
            {
                PlateNumber = plateNumber
            }, timeout: RequestTimeout.After(m: 5));

            var ride = new Ride
            {
                PictureId = pictureId,
                PlateNumber = plateNumber,
                RideDateTime = rideDateTime,
                UserId = response.Message.UserId,
                Speed = speed
            };

            await _rideRepository.AddAsync(ride);

            await _bus.Publish<RideRegistered>(new
            {
                Id = ride.Id,
                UserId = ride.UserId,
                PlateNumber = ride.PlateNumber,
                RideDateTime = ride.RideDateTime
            });

            _logger.LogInformation($"Ride registered - [ID={ride.Id}] [PlateNumber={ride.PlateNumber}] [PictureId={ride.PictureId}] [UserId={ride.UserId}] [RideDateTime={ride.RideDateTime}]");
        }

        public async Task<IEnumerable<DateTimeOffset>> GetRidesForStatisticsAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var rides = await _rideRepository.GetAllByAsync(c => c.RideDateTime >= startDate && c.RideDateTime <= endDate);
            return rides.Select(c => c.RideDateTime).ToList();
        }

        public async Task<RideDetailDTO> GetRideForFinesAsync(int rideId)
        {
            var ride = await _rideRepository.GetAsync(rideId);
            return _mapper.Map<RideDetailDTO>(ride);
        }

        private IQueryable<Ride> CreateSearchQuery(IQueryable<Ride> query, RideSearchRequest search, bool isUserRides)
        {
            if (search.SpeedLimit.HasValue)
            {
                query = query.Where(c => c.Speed > search.SpeedLimit);
            }

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
