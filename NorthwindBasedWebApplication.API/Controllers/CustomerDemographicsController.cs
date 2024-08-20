using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerDemographicsController : ControllerBase
    {
        private readonly ICustomerDemographicRepository _customerDemographicRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerDemographicsController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public CustomerDemographicsController(ICustomerDemographicRepository customerDemographicRepository, IMapper mapper,
            ILogger<CustomerDemographicsController> logger)
        {
            _customerDemographicRepository = customerDemographicRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = new();
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographics()
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographics)}")
                    .SetErrorMessage("The model state is invalid!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
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

            var customerDemographics = await _customerDemographicRepository.GetAllAsync(tracked: false);

            if(customerDemographics.Count == 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No customer demographics found in database!");

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetErrorMessage("No customer demographic found in database!")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("GET")
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

            if (customerDemographics == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographics from the database");

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographics)}")
                    .SetErrorMessage("Something went wrong while getting the customer demographics from the database")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("GET")
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

            var customerDemographicResponse = _mapper.Map<List<ReadCustomerDemographicDto>>(customerDemographics);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;

            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographics)}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("GET")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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
        [Route("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographic(int id)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetErrorMessage("The model state is invalid!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
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

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetErrorMessage("The given id is invalid")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
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

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetErrorMessage("The given id is not found!")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("GET")
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

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic from the database!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetErrorMessage("Something went wrong while getting the customer demographic from the database!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("GET")
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

            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(customerDemographic);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;
            _response.IsSuccess = true;

            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("GET")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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


        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> CreateCustomerDemographic(CreateCustomerDemographicDto createCustomerDemographicDto)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(CreateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The model states is invalid!")
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

            if (createCustomerDemographicDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(CreateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.NoContent.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The content that send by user is empty")
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

            if (await _customerDemographicRepository.IsExistAsync(cn => cn.CustomerDescription.Trim() == createCustomerDemographicDto.CustomerDescription.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The customer's description is exists, please choose another!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(CreateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The customer's description is exists, please choose another!")
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

            var customerDemographicModel = _mapper.Map<CustomerDemographic>(createCustomerDemographicDto);


            var createdCustomerDemographic = await _customerDemographicRepository.CreateAsync(customerDemographicModel);


            if (!createdCustomerDemographic)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while create customerDemographic.!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(CreateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("Something went wrong while create customer demographic.!")
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

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;


            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(CreateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("POST")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
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
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomerDemographic(int id, UpdateCustomerDemographicDto updateCustomerDemographicDto)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0 || updateCustomerDemographicDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("PUT")
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


                return BadRequest(_response);

            }

            if (id != updateCustomerDemographicDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("PUT")
                    .SetErrorMessage("No matching with given ids!")
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


            if (updateCustomerDemographicDto == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("PUT")
                    .SetErrorMessage("The content of given model is empty!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("PUT")
                    .SetErrorMessage("No customer demographic found with the given id!")
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

            var customerDemographicModel = _mapper.Map<CustomerDemographic>(updateCustomerDemographicDto);


            var updatedCustomerDemographic = await _customerDemographicRepository.UpdateAsync(customerDemographicModel);

            if (!updatedCustomerDemographic)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(UpdateCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("PUT")
                    .SetErrorMessage("Something went wrong while getting the customer demographic")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Uesr}|{Role}|{Success}{Failed}|{ErrorMessage}",
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


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("PUT")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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



        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomerDemographic(int id)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(DeleteCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(DeleteCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The given id is not valid!")
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

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer demographic with given id is not found!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(DeleteCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The customer demographic with given id is not found!")
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

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(DeleteCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("Something went wrong while getting the customer demographic")
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

            var deletedCustomerDemographic = await _customerDemographicRepository.DeleteAsync(customerDemographic);

            if (!deletedCustomerDemographic)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while deleting the customer demographic");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(DeleteCustomerDemographic)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("Something went wrong while deleting the customer demographic")
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

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;


            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("DELETE")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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
        [Route("{id:int}/Customers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> GetCustomersByTerritory(int id)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
                    .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
                    .SetErrorMessage("The given id is invalid!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("GET")
                    .SetErrorMessage("No customer demographic found with given id!")
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

            var customers = await _customerDemographicRepository.GetCustomersByCustomerDemographicAsync(id);

            if(customers.Count() == 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No Record found!");


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("GET")
                    .SetErrorMessage("No record found")
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

            if(customers == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the customers");


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("GET")
                    .SetErrorMessage("Something went wrong while getting the customers")
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

            var customersResponse = _mapper.Map<List<ReadCustomerDto>>(customers);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customersResponse;


            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{nameof(GetCustomersByTerritory)}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("GET")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                        .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
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
