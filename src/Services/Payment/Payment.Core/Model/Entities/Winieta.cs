using Shared.Core.BaseModels.Entities;
using System;

namespace Payment.Core.Model.Entities
{
    public class Winieta : BaseEntity
    {
        public int? UserId { get; set; }
        public string PaymentStatus { get; set; }
        public string PayuUrl { get; set; }
        public string OrderId { get; set; }
        public double Cost { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
