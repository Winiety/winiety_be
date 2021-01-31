using Payment.Core.Interfaces;
using Payment.Infrastructure.Data;
using Shared.Infrastructure;

namespace Payment.Infrastructure.Repositories
{
    public class WinietaRepository : BaseRepository<Core.Model.Entities.Winieta>, IWinietaRepository
    {
        public WinietaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
