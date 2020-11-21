using Moq;
using Profile.Core.Models.DTOs.UserProfile;
using Profile.Core.Models.Entities;
using Profile.Core.Models.Requests.UserProfile;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Profile.UnitTests.UserProfileServiceUnitTests
{
    public class UserProfileServiceTests : UserProfileServiceSetup
    {
        [Fact]
        public async Task GetUserProfileAsync_Should_ReturnUserProfile()
        {
            var profile = new UserProfile();
            var profileDTO = new UserProfileDTO();

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _userProfileRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), false))
                .ReturnsAsync(profile);

            _mapper
                .Setup(c => c.Map<UserProfileDTO>(It.IsAny<UserProfile>()))
                .Returns(profileDTO);

            var result = await _userProfileService.GetUserProfileAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(profileDTO, result.Result);
        }

        [Fact]
        public async Task GetUserProfileAsync_Should_CreateAndReturnUserProfile()
        {
            var profile = new UserProfile();
            var profileDTO = new UserProfileDTO();

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _userProfileRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), false))
                .ReturnsAsync((UserProfile)null);

            _userProfileRepository
                .Setup(c => c.AddAsync(It.IsAny<UserProfile>()));

            _mapper
                .Setup(c => c.Map<UserProfileDTO>(It.IsAny<UserProfile>()))
                .Returns(profileDTO);

            var result = await _userProfileService.GetUserProfileAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(profileDTO, result.Result);
            _userProfileRepository.Verify(c => c.AddAsync(It.IsAny<UserProfile>()), Times.Once());
        }

        [Fact]
        public async Task UpdateProfileAsync_Should_UpdateUserProfile()
        {
            var request = new UpdateUserProfileRequest();
            var profile = new UserProfile();
            var profileDTO = new UserProfileDTO();

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _userProfileRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), false))
                .ReturnsAsync(profile);

            _mapper
                .Setup(c => c.Map(It.IsAny<UpdateUserProfileRequest>(), It.IsAny<UserProfile>()))
                .Returns(profile);

            _mapper
                .Setup(c => c.Map<UserProfileDTO>(It.IsAny<UserProfile>()))
                .Returns(profileDTO);

            var result = await _userProfileService.UpdateProfileAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(profileDTO, result.Result);
            _userProfileRepository.Verify(c => c.AddAsync(It.IsAny<UserProfile>()), Times.Never());
        }

        [Fact]
        public async Task UpdateProfileAsync_Should_CreateAndUpdateUserProfile()
        {
            var request = new UpdateUserProfileRequest();
            var profile = new UserProfile();
            var profileDTO = new UserProfileDTO();

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _userProfileRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), false))
                .ReturnsAsync((UserProfile)null);

            _userProfileRepository
                .Setup(c => c.AddAsync(It.IsAny<UserProfile>()));

            _mapper
                .Setup(c => c.Map(It.IsAny<UpdateUserProfileRequest>(), It.IsAny<UserProfile>()))
                .Returns(profile);

            _mapper
                .Setup(c => c.Map<UserProfileDTO>(It.IsAny<UserProfile>()))
                .Returns(profileDTO);

            var result = await _userProfileService.UpdateProfileAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(profileDTO, result.Result);
            _userProfileRepository.Verify(c => c.AddAsync(It.IsAny<UserProfile>()), Times.Once());
        }
    }
}
