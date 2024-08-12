using NorthwindBasedWebApplication.API.Models.DTOs.UserDTOs;

namespace NorthwindBasedWebApplication.API.Models.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
