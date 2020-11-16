using System.Collections.Generic;

namespace Shared.Core.BaseModels.Responses.Interfaces
{
    public interface IBaseResponse
    {
        bool IsSuccess { get; }
        ICollection<Error> Errors { get; set; }
        void AddError(Error error);
    }
}
