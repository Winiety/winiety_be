namespace Shared.Core.BaseModels.Responses.Interfaces
{
    public interface IResponse<T> : IBaseResponse where T : IResponseDTO
    {
        T Result { get; set; }
    }
}
