using Microsoft.AspNetCore.Identity;

namespace NorthwindBasedWebApplication.API.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        private ApplicationRole() { }

        public ApplicationRole(string role) : base(role) 
        {

        }
    }
}
