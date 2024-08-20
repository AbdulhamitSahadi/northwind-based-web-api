using Microsoft.AspNetCore.Identity;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace NorthwindBasedWebApplication.API.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? UserIdentification { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }       
    }
}
