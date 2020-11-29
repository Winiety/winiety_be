namespace Fines.Core.Model.Requests.Complaint
{
    public class CreateComplaintRequest
    {
        public int RideId { get; set; }
        public string Description { get; set; }
    }
}
