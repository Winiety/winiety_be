namespace Shared.Core.BaseModels.Responses.Interfaces
{
    public interface IResultResponse<T> : IBaseResponse where T : IResponseDTO
    {
        T Result { get; set; }
    }
}
