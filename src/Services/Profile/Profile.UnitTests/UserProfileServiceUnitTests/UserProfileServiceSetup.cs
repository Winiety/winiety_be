using AutoMapper;
using Moq;
using Profile.Core.Interfaces;
using Profile.Core.Services;
using Shared.Core.Services;

namespace Profile.UnitTests.UserProfileServiceUnitTests
{
    public class UserProfileServiceSetup
    {
        protected readonly Mock<IUserProfileRepository> _userProfileRepository;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IMapper> _mapper;
        protected readonly UserProfileService _userProfileService;

        public UserProfileServiceSetup()
        {
            _userProfileRepository = new Mock<IUserProfileRepository>();
            _userContext = new Mock<IUserContext>();
            _mapper = new Mock<IMapper>();
            _userProfileService = new UserProfileService(_userProfileRepository.Object, _userContext.Object, _mapper.Object);
        }
    }
}
