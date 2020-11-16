using Fines.Core.Interfaces;
using Fines.Core.Model;
using Fines.Infrastructure.Data;
using Shared.Infrastructure;

namespace Fines.Infrastructure.Repositories
{
    public class FineRepository : BaseRepository<Fine>, IFineRepository
    {
        public FineRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
