using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using System.Security.Claims;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LogsController> _logger;
        private readonly ApiResponse _response;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public LogsController(ILogRepository logRepository, ILogger<LogsController> logger, IMapper mapper)
        {
            _logRepository = logRepository;
            _logger = logger;
            _mapper = mapper;
            _response = new();
            _loggingModelBuilder = new();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetLogs()
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            var logs = await _logRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states is invalid!");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLogs)}")
                    .SetFailed()
                    .SetErrorMessage("Model State is invalid!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if(logs.Count == 0)
            {
                _response.ErrorMessages.Add("No logs was found in database!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLogs)}")
                    .SetFailed()
                    .SetErrorMessage("No logs found in database!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return NotFound(_response);
            }


            if(logs == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the logs from database!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLogs)}")
                    .SetFailed()
                    .SetErrorMessage("Something went wrong while getting the logs from the database!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }


            _response.data = logs;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLogs)}")
                    .SetSuccess()
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetLog(int id)
        {

            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLog)}")
                    .SetFailed()
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetErrorMessage("The given id is invalid!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);
            }


            if(!await _logRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.ErrorMessages.Add("The log with given id is not found in database!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLog)}")
                    .SetFailed()
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .SetErrorMessage("The given id is invalid!")
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return NotFound(_response);
            }


            var log = await _logRepository.GetAsync(i => i.Id == id, tracked: false);

            if(log == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the log from the database!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLog)}")
                    .SetFailed()
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .SetErrorMessage("Something went wrong while getting the log from the database!")
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }


            _response.data = log;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;



            _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(LogsController)}/{nameof(GetLog)}")
                    .SetSuccess()
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

            _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

            return Ok(_response);
        }
    }
}
