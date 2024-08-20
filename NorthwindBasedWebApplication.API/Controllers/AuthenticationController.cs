using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.AuthDTOs;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationUserRepository;
        private readonly ApiResponse _response;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(IAuthenticationRepository authenticationUserRepository, ILogger<AuthenticationController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _authenticationUserRepository = authenticationUserRepository;
            _response = new();
            _logger = logger;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if(loginRequestDto == null)
            {
                _response.ErrorMessages.Add("No content sent it!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                

                return BadRequest(_response);
            }

            var loginUser = await _authenticationUserRepository.Login(loginRequestDto);

            if(loginUser == null)
            {
                _response.ErrorMessages.Add("Something went error while login to the system!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return BadRequest(_response);
            }

            if (string.IsNullOrWhiteSpace(loginUser.Token))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("En error with your email or password!");
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }


            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = loginUser;

            return Ok(_response);
        }


        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if(registerRequestDto == null)
            {
                _response.ErrorMessages.Add("No content sent it!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if (!await _authenticationUserRepository.IsUniqueUser(registerRequestDto.user))
            {
                _response.ErrorMessages.Add("The user is not unique!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }


            var registeredUser = await _authenticationUserRepository.Register(registerRequestDto);


            if(registeredUser == null)
            {
                _response.ErrorMessages.Add("Something went wrong while register the user");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return BadRequest(_response);
            }

            if (string.IsNullOrWhiteSpace(registeredUser.Email))
            {
                _response.ErrorMessages.Add("Failed with something!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }


            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = registeredUser;

            return Ok(_response);
        }
    }
}
