using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Models.DTOs.AuthDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.UserDTOs;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _key;

        public UserRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
            IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _key = configuration.GetValue<string>("ApiSettings:Key");
        }


        public async Task<bool> IsUniqueUser(UserDto user) 
            => !await _context.ApplicationUsers.Where(e => e.Email == user.Email).AnyAsync();

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context.ApplicationUsers
                .Where(e => e.Email.Trim() == loginRequestDto.Email.Trim())
                .FirstOrDefaultAsync()
                .GetAwaiter()
                .GetResult();

            var IsValidPassword = _userManager.CheckPasswordAsync(user, loginRequestDto.Password)
                .GetAwaiter()
                .GetResult();

            if(user == null || !IsValidPassword)
            {
                return new LoginResponseDto
                {
                    User = null,
                    Token = ""
                };
            }


            var role = _userManager.GetRolesAsync(user)
                .GetAwaiter()
                .GetResult();



            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] KeyToBytes = Encoding.UTF8.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new(new SymmetricSecurityKey(KeyToBytes), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponseDto = new()
            {
                User = _mapper.Map<UserDto>(user),
                Token = tokenHandler.WriteToken(token)
            };


            return loginResponseDto;
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                Email = registerRequestDto.user.Email,
                UserName = registerRequestDto.user.UserName,
                NormalizedEmail = registerRequestDto.user.Email.ToUpper(),
                NormalizedUserName = registerRequestDto.user.UserName.ToUpper()
            };

            try
            {
                var createdUser = await _userManager.CreateAsync(user, registerRequestDto.user.Password);


                if (createdUser.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

                        _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                    }

                    if(!await _roleManager.RoleExistsAsync("Customer"))
                    {
                        _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
                    }

                    _userManager.AddToRoleAsync(user, "Customer").GetAwaiter().GetResult();

                    var finalUser = _context.ApplicationUsers
                        .Where(e => registerRequestDto.user.Email.Trim() == e.Email.Trim())
                        .FirstOrDefaultAsync()
                        .GetAwaiter()
                        .GetResult();


                    return _mapper.Map<RegisterResponseDto>(finalUser);
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                
            }
            return null;
        }
    }
}
