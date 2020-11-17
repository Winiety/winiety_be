using Microsoft.AspNetCore.Http;
using Pictures.Core.Model.Requests.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Pictures.Core.Model.Requests
{
    public class AddPictureRequest
    {
        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile Picture { get; set; }
    }
}
