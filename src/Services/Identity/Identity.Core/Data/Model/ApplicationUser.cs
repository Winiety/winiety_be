using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Data.Model
{
    public class ApplicationUser: IdentityUser<int>
    {
        public ApplicationUser(string userName) : base(userName)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
