using Pictures.Core.Interfaces;
using Pictures.Core.Model;
using Pictures.Infrastructure.Data;

namespace Pictures.Infrastructure.Repositories
{
    public class PictureRepository : BaseRepository<Picture>, IPictureRepository
    {
        public PictureRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
