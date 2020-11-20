using System;

namespace Contracts.Events
{
    public interface RideRegistered
    {
        int Id { get; set; }
        int? UserId { get; set; }
        string PlateNumber { get; set; }
        DateTimeOffset RideDateTime { get; set; }
    }
}
