using Shared.Core.BaseModels.Requests;
using System;

namespace Payment.Core.Model.Requests
{
    public class PaymentSearchRequest : SearchRequest
    {
        public string Status { get; set; }
    }
}
