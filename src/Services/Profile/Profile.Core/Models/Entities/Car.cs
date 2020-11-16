using Shared.Core.BaseModels.Entities;

namespace Profile.Core.Models.Entities
{
    public class Car : BaseEntity
    {
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }

        public int UserId { get; set; }
    }
}
