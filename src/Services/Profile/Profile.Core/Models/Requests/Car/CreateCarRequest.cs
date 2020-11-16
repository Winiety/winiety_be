using System.ComponentModel.DataAnnotations;

namespace Profile.Core.Models.Requests.Car
{
    public class CreateCarRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "PlateNumber is required")]
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
    }
}
