using Payment.Core.Interfaces;
using Payment.Infrastructure.Data;
using Shared.Infrastructure;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Core.Model.Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
