using System;
using System.Collections.Generic;

namespace Contracts.Results
{
    public interface GetRideDatesResult
    {
        IEnumerable<DateTimeOffset> Rides { get; set; }
    }
}
