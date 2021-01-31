namespace Payment.Core.Model.Requests
{
    public class NotifyRequest
    {
        public Order order { get; set; }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
    }
}
