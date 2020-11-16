using Profile.Core.Interfaces;
using Profile.Core.Models.Entities;
using Profile.Infrastructure.Data;
using Shared.Infrastructure;

namespace Profile.Infrastructure.Repositories
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
