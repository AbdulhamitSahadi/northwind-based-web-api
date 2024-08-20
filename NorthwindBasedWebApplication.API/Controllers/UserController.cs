using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(new ApiResponse
            {
                data = users,
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                return Ok(new ApiResponse
                {
                    data = user,
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    data = null,
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateUser(ApplicationUser user)
        {
            var createUserResult = await _userRepository.CreateAsync(user);

            if (!createUserResult)
            {
                return BadRequest(new ApiResponse
                {
                    data = null,
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(ApplicationUser user)
        {
            var updateUserResult = await _userRepository.UpdateAsync(user);

            if (!updateUserResult)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
        {
            var deleteUserResult = await _userRepository.DeleteAsync(id);

            if (!deleteUserResult)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
        }


        [HttpPut]
        [Route("{id:int}/ChangePassword")]
        public async Task<ActionResult<ApiResponse>> ChangePassword(int id, [FromQuery]string currentPassword, [FromQuery] string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(id);
            bool isPasswordChanaged = await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);

            if(!isPasswordChanaged)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
        }
    }
}
