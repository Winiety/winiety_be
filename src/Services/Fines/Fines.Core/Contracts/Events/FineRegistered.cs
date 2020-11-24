using System;

namespace Contracts.Events
{
    public interface FineRegistered
    {
        int Id { get; set; }
        int UserId { get; set; }
        int RideId { get; set; }
        double Cost { get; set; }
        string Description { get; set; }
        DateTimeOffset CreateDateTime { get; set; }
    }
}
