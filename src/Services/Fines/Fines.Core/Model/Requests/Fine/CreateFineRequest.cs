namespace Fines.Core.Model.Requests.Fine
{
    public class CreateFineRequest
    {
        public int RideId { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
    }
}
