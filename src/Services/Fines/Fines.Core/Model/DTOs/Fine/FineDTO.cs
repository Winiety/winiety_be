﻿using Shared.Core.BaseModels.Responses.Interfaces;
using System;

namespace Fines.Core.Model.DTOs.Fine
{
    public class FineDTO : IResponseDTO
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public int PictureId { get; set; }
        public string PlateNumber { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
