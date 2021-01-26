using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using Fines.Core.Interfaces;
using Fines.Core.Model.DTOs.Complaint;
using Fines.Core.Model.Entities;
using Fines.Core.Model.Requests.Complaint;
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
    public interface IComplaintService
    {
        Task<IResultResponse<ComplaintDetailDTO>> GetComplaintAsync(int id);
        Task<IPagedResponse<ComplaintDetailDTO>> GetComplaintsAsync(SearchRequest searchRequest);
        Task<IPagedResponse<ComplaintDTO>> GetUserComplaintsAsync(SearchRequest searchRequest);
        Task<IResultResponse<ComplaintDTO>> CreateComplaintAsync(CreateComplaintRequest complaint);
        Task<IResultResponse<ComplaintDTO>> UpdateComplaintAsync(UpdateComplaintRequest complaint);
        Task<IBaseResponse> RemoveComplaintAsync(int id);
    }

    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IBusControl _bus;
        private readonly IUserContext _userContext;
        private readonly IRequestClient<GetRide> _requestClient;
        private readonly IMapper _mapper;

        public ComplaintService(IComplaintRepository complaintRepository, IBusControl bus, IUserContext userContext, IRequestClient<GetRide> requestClient, IMapper mapper)
        {
            _complaintRepository = complaintRepository;
            _bus = bus;
            _userContext = userContext;
            _requestClient = requestClient;
            _mapper = mapper;
        }

        public async Task<IResultResponse<ComplaintDTO>> CreateComplaintAsync(CreateComplaintRequest complaint)
        {
            var response = new ResultResponse<ComplaintDTO>();

            var (rideResponse, rideNotFoundResponse) = await _requestClient.GetResponse<GetRideResult, GetRideNotFound>(new
            {
                RideId = complaint.RideId
            });

            if (rideNotFoundResponse.IsCompletedSuccessfully)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono przejazdu"
                });

                return response;
            }

            var ride = (await rideResponse).Message;

            var currentUserId = _userContext.GetUserId();

            if (!ride.UserId.HasValue)
            {
                response.AddError(new Error
                {
                    Message = "User id jest pusty dla zarejestrowanego przejazdu"
                });

                return response;
            }

            if (ride.UserId != currentUserId)
            {
                response.AddError(new Error
                {
                    Message = "User id nie pasuje"
                });

                return response;
            }

            var complaintEntity = new Complaint
            {
                RideId = ride.Id,
                PictureId = ride.PictureId,
                UserId = ride.UserId.Value,
                PlateNumber = ride.PlateNumber,
                CreateTime = DateTimeOffset.UtcNow,
                Description = complaint.Description
            };

            await _complaintRepository.AddAsync(complaintEntity);

            await _bus.Publish<ComplaintRegistered>(new
            {
                Id = complaintEntity.Id,
                UserId = complaintEntity.UserId,
                RideId = complaintEntity.RideId,
                Description = complaintEntity.Description,
                CreateDateTime = complaintEntity.CreateTime
            });

            response.Result = _mapper.Map<ComplaintDTO>(complaintEntity);

            return response;
        }

        public async Task<IResultResponse<ComplaintDetailDTO>> GetComplaintAsync(int id)
        {
            var response = new ResultResponse<ComplaintDetailDTO>();

            var complaintEntity = await _complaintRepository.GetAsync(id);

            if (complaintEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono skargi"
                });

                return response;
            }

            response.Result = _mapper.Map<ComplaintDetailDTO>(complaintEntity);

            return response;
        }

        public async Task<IPagedResponse<ComplaintDetailDTO>> GetComplaintsAsync(SearchRequest search)
        {
            var response = new PagedResponse<ComplaintDetailDTO>();

            var complaintsQuery = _complaintRepository.GetQueryable();
            complaintsQuery = CreateSearchQuery(complaintsQuery, search, false);

            var complaints = await _complaintRepository.GetPagedByAsync(
                 complaintsQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(complaints, response);

            return response;
        }

        public async Task<IPagedResponse<ComplaintDTO>> GetUserComplaintsAsync(SearchRequest search)
        {
            var response = new PagedResponse<ComplaintDTO>();

            var complaintsQuery = _complaintRepository.GetQueryable();
            complaintsQuery = CreateSearchQuery(complaintsQuery, search, true);

            var complaints = await _complaintRepository.GetPagedByAsync(
                 complaintsQuery,
                 search.PageNumber,
                 search.PageSize);

            response = _mapper.Map(complaints, response);

            return response;
        }

        public async Task<IBaseResponse> RemoveComplaintAsync(int id)
        {
            var response = new BaseResponse();

            var complaint = await _complaintRepository.GetAsync(id);

            if (complaint == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono skargi"
                });

                return response;
            }

            var currentUserId = _userContext.GetUserId();

            if (complaint.UserId != currentUserId)
            {
                response.AddError(new Error
                {
                    Message = "User id nie pasuje"
                });

                return response;
            }

            await _complaintRepository.RemoveAsync(complaint);

            return response;
        }

        public async Task<IResultResponse<ComplaintDTO>> UpdateComplaintAsync(UpdateComplaintRequest complaint)
        {
            var response = new ResultResponse<ComplaintDTO>();

            var complaintEntity = await _complaintRepository.GetAsync(complaint.Id);

            if (complaintEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Nie znaleziono skargi"
                });

                return response;
            }

            var currentUserId = _userContext.GetUserId();

            if (complaintEntity.UserId != currentUserId)
            {
                response.AddError(new Error
                {
                    Message = "User id nie pasuje"
                });

                return response;
            }

            complaintEntity.Description = complaint.Description;

            await _complaintRepository.UpdateAsync(complaintEntity);

            response.Result = _mapper.Map<ComplaintDTO>(complaintEntity);

            return response;
        }

        private IQueryable<Complaint> CreateSearchQuery(IQueryable<Complaint> query, SearchRequest search, bool isUserComplaints)
        {
            if (isUserComplaints)
            {
                var currentUserId = _userContext.GetUserId();
                query = query.Where(c => c.UserId == currentUserId);
            }

            var searchQuery = search.Query;

            return query.Where(c => c.Description.Contains(searchQuery) || c.PlateNumber.Contains(searchQuery));
        }
    }
}
