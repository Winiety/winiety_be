using System;

namespace Rides.Core.Model
{
    public class Ride : BaseEntity
    {
        public int? UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
    }
}
