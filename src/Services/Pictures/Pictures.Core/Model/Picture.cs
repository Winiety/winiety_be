﻿using Shared.Core.BaseModels.Entities;

namespace Pictures.Core.Model
{
    public class Picture : BaseEntity
    {
        public string ImagePath { get; set; }
        public string PlateNumber { get; set; }
        public bool IsRecognized { get; set; }
    }
}
