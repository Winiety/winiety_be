using Shared.Core.BaseModels.Responses.Interfaces;
using System;

namespace Payment.Core.Model.DTOs
{
    public class WinietaDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string PaymentStatus { get; set; }
        public string PayuUrl { get; set; }
        public double Cost { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
