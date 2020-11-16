using Shared.Core.BaseModels.Responses.Interfaces;

namespace Profile.Core.Models.DTOs.Car
{
    public class CarDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
    }
}
