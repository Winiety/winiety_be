using Profile.Core.Interfaces;
using Profile.Core.Models.Entities;
using Profile.Infrastructure.Data;
using Shared.Infrastructure;

namespace Profile.Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
