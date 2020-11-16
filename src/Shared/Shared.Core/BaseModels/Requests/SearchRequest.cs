namespace Shared.Core.BaseModels.Requests
{
    public class SearchRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Query { get; set; } = string.Empty;
    }
}
