using NorthwindBasedWebApplication.API.Models.DTOs.AuthDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.UserDTOs;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUser(UserDto user);
        Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
