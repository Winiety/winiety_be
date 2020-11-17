namespace Rides.Core.Model.DTOs
{
    public class RideDetailDTO : RideDTO
    {
        public int? UserId { get; set; }
        public int PictureId { get; set; }
    }
}
