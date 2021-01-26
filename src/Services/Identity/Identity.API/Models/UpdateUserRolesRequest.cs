namespace Identity.API.Models
{
    public class UpdateUserRolesRequest
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; }
    }
}
