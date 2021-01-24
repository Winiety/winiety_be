namespace Notification.Core.Model.Requests
{
    public class RegisterSubscriptionRequest
    {
        public string Endpoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
