using System.Collections.Generic;

namespace Shared.Core.Interfaces
{
    public interface IPagedList<T> : IList<T>
    {
        int PageSize { get; set; }
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int TotalCount { get; set; }
    }
}
