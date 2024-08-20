using System.Security.Claims;

namespace NorthwindBasedWebApplication.API.Models.Requests
{
    public static class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            new Claim("Create Product", "false"),
            new Claim("Update Product", "false"),
            new Claim("Delete Product", "false")
        };
    }
}
