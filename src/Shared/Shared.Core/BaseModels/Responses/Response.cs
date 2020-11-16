using Shared.Core.BaseModels.Responses.Interfaces;

namespace Shared.Core.BaseModels.Responses
{
    public class Response<T> : BaseResponse, IResponse<T> where T : IResponseDTO
    {
        public T Result { get; set; }
    }
}
