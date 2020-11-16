namespace Shared.Core.BaseModels.Responses.Interfaces
{
    public interface IPagedResponse<T> : ICollectionResponse<T> where T : IResponseDTO
    {
        int PageSize { get; set; }
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int TotalCount { get; set; }
    }
}
