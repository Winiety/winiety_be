using System;

namespace Contracts.Events
{
    public interface CarRegistered
    {
        int PictureId { get; set; }
        string PlateNumber { get; set; }
        double Speed { get; set; }
        DateTimeOffset RideDateTime { get; set; }
    }
}
