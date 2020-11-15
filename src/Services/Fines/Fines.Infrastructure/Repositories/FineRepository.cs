using Fines.Core.Interfaces;
using Fines.Core.Model;
using Fines.Infrastructure.Data;

namespace Fines.Infrastructure.Repositories
{
    public class FineRepository : BaseRepository<Fine>, IFineRepository
    {
        public FineRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
