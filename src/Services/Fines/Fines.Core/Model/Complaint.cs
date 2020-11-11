namespace Fines.Core.Model
{
    public class Complaint: BaseEntity
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public string Description { get; set; }
    }
}
