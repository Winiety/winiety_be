using Shared.Core.BaseModels.Responses.Interfaces;

namespace Shared.Core.BaseModels.Responses
{
    public class PagedResponse<T> : CollectionResponse<T>, IPagedResponse<T> where T : IResponseDTO
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
