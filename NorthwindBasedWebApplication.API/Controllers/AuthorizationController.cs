using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.Common.Authorization;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ApiResponse _response;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationController(IAuthorizationRepository authorizationRepository,
            ILogger<AuthorizationController> logger, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _authorizationRepository = authorizationRepository;
            _response = new ApiResponse();
            _logger = logger;
            _loggingModelBuilder = new();
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("Roles")]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            var roles = await _authorizationRepository.GetAllAsync();

            if (!roles.Any())
            {
                _response.ErrorMessages.Add("No roles was found!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetErrorMessage("No roles was found!")
                    .SetFailed()
                    .SetMethodType("GET")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRoles)}")
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return NotFound(_response);
            }

            if(roles == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the roles!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetErrorMessage("Something went wrong while getting the roles!")
                    .SetFailed()
                    .SetMethodType("GET")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRoles)}")
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = roles;


            _loggingModelBuilder
                .SetSuccess()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.OK.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRoles)}")
                .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);

            return Ok(_response);
        }

        [HttpGet]
        [Route("Roles/{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetRole(int id)
        {
            if(id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
                .SetErrorMessage("The given id is invalid!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            if (!await _authorizationRepository.IsExistsAsync(id))
            {
                _response.ErrorMessages.Add("The role with given id is not found!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
                .SetErrorMessage("The role with given id is not found!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var role = await _authorizationRepository.GetByIdAsync(id);

            if(role == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the role!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
                .SetErrorMessage("Something went wrong while getting the role with given id!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = false;
            _response.data = role;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("GET")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);

        }


        [HttpPut]
        [Route("Roles/{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateRole(int id, Models.Common.UpdateRoleRequest updateRoleRequest)
        {
            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("PUT")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(UpdateRole)}")
                .SetErrorMessage("The given id is invalid!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (id != updateRoleRequest.Id)
            {
                _response.ErrorMessages.Add("No matching between ids!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("PUT")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(UpdateRole)}")
                .SetErrorMessage("No matching between ids!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (updateRoleRequest == null)
            {
                _response.ErrorMessages.Add("No content sent by user for updating!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("PUT")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(UpdateRole)}")
                .SetErrorMessage("No content sent by ser for updating!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            

            if (!await _authorizationRepository.IsExistsAsync(id))
            {
                _response.ErrorMessages.Add("The role with given id is not found!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
                .SetErrorMessage("The role with given id is not found!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }



            var updateRoleResult = await _authorizationRepository.UpdateRoleAsync(updateRoleRequest);

            if (!updateRoleResult)
            {
                _response.ErrorMessages.Add("Something went wrong while updating the role with given id!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("PUT")
                .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(UpdateRole)}")
                .SetErrorMessage("Something went wrong while updating the role with given id!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response) ;
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("PUT")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRole)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);

        }

        [HttpPost]
        [Route("Roles")]
        public async Task<ActionResult<ApiResponse>> CreateRole([FromBody] CreateRoleRequest createRoleRequest)
        {
            if (createRoleRequest == null)
            {
                _response.ErrorMessages.Add("The content for creating the role is empty!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("POST")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(CreateRole)}")
                .SetErrorMessage("The content for creating the role is empty!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (await _authorizationRepository.IsExistsAsync(createRoleRequest.Name))
            {
                _response.ErrorMessages.Add("The role's name is exists!, please choose another");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("POST")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(CreateRole)}")
                .SetErrorMessage("The role's name is exists!, please choose another")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var createRoleResult = await _authorizationRepository.AddRoleAsync(createRoleRequest.Name);

            if (createRoleRequest == null)
            {
                _response.ErrorMessages.Add("Something went wrong while creating the role!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("POST")
                .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(UpdateRole)}")
                .SetErrorMessage("Something went wrong while creating the role!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("POST")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(CreateRole)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }


        [HttpDelete]
        [Route("Roles/{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteRole(int id)
        {
            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("DELETE")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(DeleteRole)}")
                .SetErrorMessage("The given id is invalid!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (!await _authorizationRepository.IsExistsAsync(id))
            {
                _response.ErrorMessages.Add("No roles exists with the given id!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("POST")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(DeleteRole)}")
                .SetErrorMessage("No roles exists with the given id!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var deleteRoleResult = await _authorizationRepository.DeleteRoleAsync(id);

            if (!deleteRoleResult)
            {
                _response.ErrorMessages.Add("Something went wrong while deleting the role!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("DELETE")
                .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(DeleteRole)}")
                .SetErrorMessage("Something went wrong while deleting the role!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("DELETE")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(DeleteRole)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }



        [HttpGet]
        [Route("User/{id:int}/Roles")]
        public async Task<ActionResult<ApiResponse>> GetRolesByUser(int id)
        {
            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRolesByUser)}")
                .SetErrorMessage("The given id is invalid!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (await _userManager.FindByIdAsync(id.ToString()) == null)
            {
                _response.ErrorMessages.Add("No user exists with the given id!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRolesByUser)}")
                .SetErrorMessage("No user exists with the given id!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var roles = await _authorizationRepository.GetRolesByUser(id);

            if (roles.Roles.Count == 0)
            {
                _response.ErrorMessages.Add("No roles found by given user!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRolesByUser)}")
                .SetErrorMessage("Something went wrong while deleting the role!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (roles == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the roles by the user!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRolesByUser)}")
                .SetErrorMessage("Something went wrong while getting the roles by the user!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = roles;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("GET")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetRolesByUser)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }


        [HttpGet]
        [Route("User/{id:int}/Claims")]
        public async Task<ActionResult<ApiResponse>> GetClaimsByUser(int id)
        {
            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetClaimsByUser)}")
                .SetErrorMessage("The given id is invalid!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (await _userManager.FindByIdAsync(id.ToString()) == null)
            {
                _response.ErrorMessages.Add("No user exists with the given id!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetClaimsByUser)}")
                .SetErrorMessage("No user exists with the given id!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var claims = await _authorizationRepository.GetClaimsByUser(id);

            if (claims.Claims.Count == 0)
            {
                _response.ErrorMessages.Add("No claims found by given user!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetClaimsByUser)}")
                .SetErrorMessage("No claims found by given user!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (claims == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the claims by the user!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;

                _loggingModelBuilder
                .SetFailed()
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.NotFound.ToString())
                .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetClaimsByUser)}")
                .SetErrorMessage("Something went wrong while getting the claims by the user!")
                .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = claims;

            _loggingModelBuilder
            .SetFailed()
            .SetMethodType("GET")
            .SetStatusCode(HttpStatusCode.OK.ToString())
            .SetDetails($"{nameof(AuthorizationController)}/{nameof(GetClaimsByUser)}")
            .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                _loggingModelBuilder.Build().StatusCode,
                _loggingModelBuilder.Build().MethodType,
                _loggingModelBuilder.Build().Role,
                _loggingModelBuilder.Build().Success,
                _loggingModelBuilder.Build().Failed,
                _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }

    }
}
