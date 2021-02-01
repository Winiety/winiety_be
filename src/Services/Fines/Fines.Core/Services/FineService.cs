using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using Fines.Core.Interfaces;
using Fines.Core.Model.DTOs.Fine;
using Fines.Core.Model.Entities;
using Fines.Core.Model.Requests.Fine;
using MassTransit;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fines.Core.Services
{
    public interface IFineService
    {
        Task<IResultResponse<FineDetailDTO>> GetFineAsync(int id);
        Task<IPagedResponse<FineDetailDTO>> GetFinesAsync(SearchRequest searchRequest);
        Task<IPagedResponse<FineDTO>> GetUserFinesAsync(SearchRequest searchRequest);
        Task<IResultResponse<FineDTO>> CreateFineAsync(CreateFineRequest fine);
        Task<IResultResponse<FineDTO>> UpdateFineAsync(UpdateFineRequest fine);
        Task<IBaseResponse> RemoveFineAsync(int id);
    }

    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepository;
        private readonly IBusControl _bus;
        private readonly IUserContext _userContext;
        private readonly IRequestClient<GetRide> _requestClient;
        private readonly IMapper _mapper;

        public FineService(IFineRepository fineRepository, IBusControl bus, IUserContext userContext, IRequestClient<GetRide> requestClient, IMapper mapper)
        {
            _fineRepository = fineRepository;
            _bus = bus;
            _userContext = userContext;
            _requestClient = requestClient;
            _mapper = mapper;
        }

        public async Task<IResultResponse<FineDTO>> CreateFineAsync(CreateFineRequest fine)
        {
            var response = new ResultResponse<FineDTO>();

            var (rideResponse, rideNotFoundResponse) = await _requestClient.GetResponse<GetRideResult, GetRideNotFound>(new
            {
                RideId = fine.RideId
            }, timeout: RequestTimeout.After(m: 5));

            if (rideNotFoundResponse.IsCompletedSuccessfully)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono przejazdu"
                });

                return response;
            }

            var ride = (await rideResponse).Message;

            if (!ride.UserId.HasValue)
            {
                response.AddError(new Error
                {
                    Message = "User id jest pusty dla zarejestrowanego przejazdu"
                });

                return response;
            }

            var fineEntity = new Fine
            {
                RideId = ride.Id,
                PictureId = ride.PictureId,
                UserId = ride.UserId.Value,
                PlateNumber = ride.PlateNumber,
                CreateTime = DateTimeOffset.UtcNow,
                Cost = fine.Cost,
                Description = fine.Description
            };

            await _fineRepository.AddAsync(fineEntity);

            await _bus.Publish<FineRegistered>(new
            {
                Id = fineEntity.Id,
                UserId = fineEntity.UserId,
                RideId = fineEntity.RideId,
                Description = fineEntity.Description,
                Cost = fineEntity.Cost,
                CreateDateTime = fineEntity.CreateTime
            });

            response.Result = _mapper.Map<FineDTO>(fineEntity);

            return response;
        }

        public async Task<IResultResponse<FineDetailDTO>> GetFineAsync(int id)
        {
            var response = new ResultResponse<FineDetailDTO>();

            var fineEntity = await _fineRepository.GetAsync(id);

            if (fineEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono mandatu"
                });

                return response;
            }

            response.Result = _mapper.Map<FineDetailDTO>(fineEntity);

            return response;
        }

        public async Task<IPagedResponse<FineDetailDTO>> GetFinesAsync(SearchRequest search)
        {
            var response = new PagedResponse<FineDetailDTO>();

            var finesQuery = _fineRepository.GetQueryable();
            finesQuery = CreateSearchQuery(finesQuery, search, false);
            finesQuery.OrderByDescending(c => c.CreateTime);

            var fines = await _fineRepository.GetPagedByAsync(
                 finesQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(fines, response);

            return response;
        }

        public async Task<IPagedResponse<FineDTO>> GetUserFinesAsync(SearchRequest search)
        {
            var response = new PagedResponse<FineDTO>();

            var finesQuery = _fineRepository.GetQueryable();
            finesQuery = CreateSearchQuery(finesQuery, search, true);
            finesQuery.OrderByDescending(c => c.CreateTime);

            var fines = await _fineRepository.GetPagedByAsync(
                 finesQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(fines, response);

            return response;
        }

        public async Task<IBaseResponse> RemoveFineAsync(int id)
        {
            var response = new BaseResponse();

            var fine = await _fineRepository.GetAsync(id);

            if (fine == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono mandatu"
                });

                return response;
            }

            await _fineRepository.RemoveAsync(fine);

            return response;
        }

        public async Task<IResultResponse<FineDTO>> UpdateFineAsync(UpdateFineRequest fine)
        {
            var response = new ResultResponse<FineDTO>();

            var fineEntity = await _fineRepository.GetAsync(fine.Id);

            if (fineEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono mandatu"
                });

                return response;
            }

            fineEntity.Description = fine.Description;
            fineEntity.Cost = fine.Cost;

            await _fineRepository.UpdateAsync(fineEntity);

            response.Result = _mapper.Map<FineDTO>(fineEntity);

            return response;
        }

        private IQueryable<Fine> CreateSearchQuery(IQueryable<Fine> query, SearchRequest search, bool isUserFines)
        {
            if (isUserFines)
            {
                var currentUserId = _userContext.GetUserId();
                query = query.Where(c => c.UserId == currentUserId);
            }

            var searchQuery = search.Query;

            return query.Where(c => c.Cost.ToString() == searchQuery || c.PlateNumber.Contains(searchQuery));
        }
    }
}
