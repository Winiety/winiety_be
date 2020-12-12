using Shared.Core.BaseModels.Entities;
using System;

namespace Fines.Core.Model.Entities
{
    public class Fine : BaseEntity
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
