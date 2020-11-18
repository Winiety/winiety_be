using Shared.Core.BaseModels.Responses.Interfaces;

namespace Shared.Core.BaseModels.Responses
{
    public class ResultResponse<T> : BaseResponse, IResultResponse<T> where T : IResponseDTO
    {
        public T Result { get; set; }
    }
}
