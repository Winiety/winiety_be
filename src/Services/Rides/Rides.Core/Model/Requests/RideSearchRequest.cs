using Shared.Core.BaseModels.Requests;
using System;

namespace Rides.Core.Model.Requests
{
    public class RideSearchRequest : SearchRequest
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
