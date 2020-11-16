using AutoMapper;
using Profile.Core.Interfaces;
using Profile.Core.Models.DTOs.UserProfile;
using Profile.Core.Models.Entities;
using Profile.Core.Models.Requests.UserProfile;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System.Threading.Tasks;

namespace Profile.Core.Services
{
    public interface IUserProfileService
    {
        Task<IResponse<UserProfileDTO>> UpdateProfileAsync(UpdateUserProfileRequest userProfile);
        Task<IResponse<UserProfileDTO>> GetUserProfileAsync();
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public UserProfileService(IUserProfileRepository userProfileRepository, IUserContext userContext, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _userContext = userContext;
            _mapper = mapper;
        }


        public async Task<IResponse<UserProfileDTO>> UpdateProfileAsync(UpdateUserProfileRequest userProfile)
        {
            var response = new Response<UserProfileDTO>();
            var currentUserId = _userContext.GetUserId();

            var profileEntity = await _userProfileRepository.GetByAsync(c => c.UserId == currentUserId);

            if (profileEntity == null)
            {
                profileEntity = new UserProfile
                {
                    UserId = currentUserId,
                };

                await _userProfileRepository.AddAsync(profileEntity);
            }

            _mapper.Map(userProfile, profileEntity);

            await _userProfileRepository.UpdateAsync(profileEntity);

            response.Result = _mapper.Map<UserProfileDTO>(profileEntity);

            return response;
        }

        public async Task<IResponse<UserProfileDTO>> GetUserProfileAsync()
        {
            var response = new Response<UserProfileDTO>();
            var currentUserId = _userContext.GetUserId();

            var profileEntity = await _userProfileRepository.GetByAsync(c => c.UserId == currentUserId);

            if (profileEntity == null)
            {
                profileEntity = new UserProfile
                {
                    UserId = currentUserId,
                };

                await _userProfileRepository.AddAsync(profileEntity);
            }

            response.Result = _mapper.Map<UserProfileDTO>(profileEntity);

            return response;
        }
    }
}
