namespace Fines.Core.Model
{
    public class Fine : BaseEntity
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public double Cost { get; set; }
    }
}
