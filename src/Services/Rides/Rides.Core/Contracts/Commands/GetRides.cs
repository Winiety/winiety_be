using System;

namespace Contracts.Commands
{
    public interface GetRides
    {
        DateTimeOffset DateFrom { get; set; }
        DateTimeOffset DateTo { get; set; }
    }
}
