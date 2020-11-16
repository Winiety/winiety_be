using Shared.Core.BaseModels.Responses.Interfaces;
using System.Collections.Generic;

namespace Shared.Core.BaseModels.Responses
{
    public class BaseResponse : IBaseResponse
    {
        public bool IsSuccess { get => Errors.Count == 0; }

        public ICollection<Error> Errors { get; set; } = new List<Error>();

        public void AddError(Error error)
        {
            Errors.Add(error);
        }
    }
}
