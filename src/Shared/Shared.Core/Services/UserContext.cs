using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Shared.Core.Services
{
    public interface IUserContext
    {
        int GetUserId();
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var id = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            if (string.IsNullOrWhiteSpace(id) || !int.TryParse(id, out int result))
            {
                throw new UnauthorizedAccessException();
            }

            return result;
        }
    }
}
