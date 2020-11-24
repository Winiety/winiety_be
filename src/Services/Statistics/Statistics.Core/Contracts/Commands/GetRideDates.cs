using System;

namespace Contracts.Commands
{
    public interface GetRideDates
    {
        DateTimeOffset DateFrom { get; set; }
        DateTimeOffset DateTo { get; set; }
    }
}
