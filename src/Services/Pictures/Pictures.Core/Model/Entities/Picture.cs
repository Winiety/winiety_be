using Shared.Core.BaseModels.Entities;
using System;

namespace Pictures.Core.Model.Entities
{
    public class Picture : BaseEntity
    {
        public string ImagePath { get; set; }
        public string PlateNumber { get; set; }
        public bool IsRecognized { get; set; }
        public double Speed { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
    }
}
