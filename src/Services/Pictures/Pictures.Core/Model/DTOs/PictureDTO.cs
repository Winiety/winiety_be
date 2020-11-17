using Shared.Core.BaseModels.Responses.Interfaces;

namespace Pictures.Core.Model.DTOs
{
    public class PictureDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string PlateNumber { get; set; }
        public bool IsRecognized { get; set; }
    }
}
