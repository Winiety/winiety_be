using System;

namespace Contracts.Results
{
    public interface GetRideResult
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
    }
}
