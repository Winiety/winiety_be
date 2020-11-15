using Fines.Core.Interfaces;
using Fines.Core.Model;
using Fines.Infrastructure.Data;

namespace Fines.Infrastructure.Repositories
{
    public class ComplaintRepository : BaseRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
