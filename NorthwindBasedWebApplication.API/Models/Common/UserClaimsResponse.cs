namespace NorthwindBasedWebApplication.API.Models.Common
{
    public class UserClaimsResponse
    {
        public int UserId { get; set; }
        public List<UserClaims> Claims { get; set; }
    }
}
