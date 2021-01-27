using Shared.Core.BaseModels.Responses.Interfaces;
using System;

namespace Rides.Core.Model.DTOs
{
    public class RideDTO : IResponseDTO
    {
        public int Id { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
        public double Speed { get; set; }
    }
}
