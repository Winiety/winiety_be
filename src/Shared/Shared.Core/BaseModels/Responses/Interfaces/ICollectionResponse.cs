using System.Collections.Generic;

namespace Shared.Core.BaseModels.Responses.Interfaces
{
    public interface ICollectionResponse<T> : IBaseResponse where T : IResponseDTO
    {
        IEnumerable<T> Results { get; set; }
    }
}
