namespace Payment.Core.Model.Requests
{
    public class PayRequest
    {
        public int PaymentId { get; set; }
        public string continueUrl { get; set; }
    }
}
