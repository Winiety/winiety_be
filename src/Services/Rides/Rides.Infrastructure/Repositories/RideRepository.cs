using Rides.Core.Interfaces;
using Rides.Core.Model;
using Rides.Infrastructure.Data;

namespace Rides.Infrastructure.Repositories
{
    public class RideRepository : BaseRepository<Ride>, IRideRepository
    {
        public RideRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
