using Shared.Core.BaseModels.Entities;
using System;

namespace Rides.Core.Model.Entities
{
    public class Ride : BaseEntity
    {
        public int? UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
        public double Speed { get; set; }
    }
}
