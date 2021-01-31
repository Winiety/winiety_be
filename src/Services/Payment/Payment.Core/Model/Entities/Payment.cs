using Shared.Core.BaseModels.Entities;

namespace Payment.Core.Model.Entities
{
    public class Payment : BaseEntity
    {
        public int RideId { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; }
        public string PayuUrl { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
    }
}
