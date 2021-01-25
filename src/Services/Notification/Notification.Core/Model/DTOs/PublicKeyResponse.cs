using Shared.Core.BaseModels.Responses.Interfaces;

namespace Notification.Core.Model.DTOs
{
    public class PublicKeyResponse : IResponseDTO
    {
        public string PublicKey { get; set; }
    }
}
