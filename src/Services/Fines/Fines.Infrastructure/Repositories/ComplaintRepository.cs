using Fines.Core.Interfaces;
using Fines.Core.Model.Entities;
using Fines.Infrastructure.Data;
using Shared.Infrastructure;

namespace Fines.Infrastructure.Repositories
{
    public class ComplaintRepository : BaseRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
