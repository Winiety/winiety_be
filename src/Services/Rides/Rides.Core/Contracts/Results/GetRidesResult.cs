using System;
using System.Collections.Generic;

namespace Contracts.Results
{
    public interface GetRidesResult
    {
        IEnumerable<DateTimeOffset> Rides { get; set; }
    }
}
