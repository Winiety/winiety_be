namespace Fines.Core.Model.Requests.Fine
{
    public class UpdateFineRequest
    {
        public int Id { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
    }
}
