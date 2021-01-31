using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Model.DTOs
{
    public class PaymentDetailDTO : PaymentDTO
    {
        public int? UserId { get; set; }
    }
}
