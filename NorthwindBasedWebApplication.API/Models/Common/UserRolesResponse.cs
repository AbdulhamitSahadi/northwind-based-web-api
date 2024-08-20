namespace NorthwindBasedWebApplication.API.Models.Common
{
    public class UserRolesResponse
    {
        public int UserId { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
