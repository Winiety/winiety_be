using Shared.Core.BaseModels.Responses.Interfaces;

namespace Payment.Core.Model.DTOs
{
    public class PaymentDTO : IResponseDTO
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public string PayuUrl { get; set; }
    }
}
