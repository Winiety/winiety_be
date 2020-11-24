using System;

namespace Contracts.Events
{
    public interface ComplaintRegistered
    {
        int Id { get; set; }
        int UserId { get; set; }
        int RideId { get; set; }
        string Description { get; set; }
        DateTimeOffset CreateDateTime { get; set; }
    }
}
