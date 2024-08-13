using Microsoft.AspNetCore.Identity;

namespace NorthwindBasedWebApplication.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? UserIdentification { get; set; }
    }
}
