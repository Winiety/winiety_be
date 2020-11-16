using Shared.Core.BaseModels.Responses.Interfaces;
using System.Collections.Generic;

namespace Shared.Core.BaseModels.Responses
{
    public class CollectionResponse<T> : BaseResponse, ICollectionResponse<T> where T : IResponseDTO
    {
        public IEnumerable<T> Results { get; set; }
    }
}
