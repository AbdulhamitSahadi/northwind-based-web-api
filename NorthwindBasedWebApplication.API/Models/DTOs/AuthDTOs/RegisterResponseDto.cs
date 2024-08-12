using NorthwindBasedWebApplication.API.Models.DTOs.UserDTOs;

namespace NorthwindBasedWebApplication.API.Models.DTOs.AuthDTOs
{
    public class RegisterResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
