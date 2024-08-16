using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs;
using System.Security.Principal;

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
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographics()
        {

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
                    .Build();

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
                    .Build();

                return NotFound(_response);
            }

            if (customerDemographics == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographics from the database");

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographics}")
                    .SetErrorMessage("Something went wrong while getting the customer demographics from the database")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("GET")
                    .Build();

                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<List<ReadCustomerDemographicDto>>(customerDemographics);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;

            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographics}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("GET")
                    .Build();


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographic(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographic}")
                    .SetErrorMessage("The model state is invalid!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
                    .Build();

                return BadRequest(_response);
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographic}")
                    .SetErrorMessage("The given id is invalid")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("GET")
                    .Build();

                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographic}")
                    .SetErrorMessage("The given id is not found!")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("GET")
                    .Build();

                return NotFound(_response);
            }

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic from the database!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographic}")
                    .SetErrorMessage("Something went wrong while getting the customer demographic from the database!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("GET")
                    .Build();

                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(customerDemographic);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;
            _response.IsSuccess = true;

            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomerDemographic}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("GET")
                    .Build();

            return Ok(_response);
        }


        

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(CreateCustomerDemographicDto createCustomerDemographicDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{Create}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The model states is invalid!")
                    .Build();


                return BadRequest(_response);
            }

            if (createCustomerDemographicDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{Create}")
                    .SetStatusCode(HttpStatusCode.NoContent.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The content that send by user is empty")
                    .Build();

                return BadRequest(_response);
            }

            if (await _customerDemographicRepository.IsExistAsync(cn => cn.CustomerDescription.Trim() == createCustomerDemographicDto.CustomerDescription.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The customer's description is exists, please choose another!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{Create}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("The customer's description is exists, please choose another!")
                    .Build();


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
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{Create}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("POST")
                    .SetErrorMessage("Something went wrong while create customer demeographic.!")
                    .Build();


                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;


            _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CustomerDemographicsController)}/{Create}")
                    .SetStatusCode(HttpStatusCode.OK.ToString())
                    .SetMethodType("POST")
                    .Build();


            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomerDemographic(int id, UpdateCustomerDemographicDto updateCustomerDemegraphicDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("The model state is invalid!")
                        .Build();

                return BadRequest(_response);
            }

            if (id <= 0 || updateCustomerDemegraphicDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("The given id is invalid!")
                        .Build();


                return BadRequest(_response);

            }

            if (id != updateCustomerDemegraphicDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("No matching with given ids!")
                        .Build();


                return BadRequest(_response);
            }


            if (updateCustomerDemegraphicDto == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.NoContent.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("The content of given model is empty!")
                        .Build();


                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.NotFound.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("No customer demographic found with the given id!")
                        .Build();


                return NotFound(_response);
            }

            var customerDemographicModel = _mapper.Map<CustomerDemographic>(updateCustomerDemegraphicDto);


            var updatedCustomerDemographic = await _customerDemographicRepository.UpdateAsync(customerDemographicModel);

            if (!updatedCustomerDemographic)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                        .SetMethodType("PUT")
                        .SetErrorMessage("Something went wrong while getting the customer demographic")
                        .Build();


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                        .SetSuccess()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{UpdateCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.OK.ToString())
                        .SetMethodType("PUT")
                        .Build();

            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomerDemographic(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("DELETE")
                        .SetErrorMessage("The model state is invalid!")
                        .Build();

                return BadRequest(_response);
            }

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("DELETE")
                        .SetErrorMessage("The given id is not valid!")
                        .Build();

                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer demographic with given id is not found!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.NotFound.ToString())
                        .SetMethodType("DELETE")
                        .SetErrorMessage("The customer demographic with given id is not found!")
                        .Build();


                return NotFound(_response);
            }

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the customer demographic");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                        .SetMethodType("DELETE")
                        .SetErrorMessage("Something went wrong while getting the customer demographic")
                        .Build();


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
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                        .SetMethodType("DELETE")
                        .SetErrorMessage("Something went wrong while deleting the customer demographic")
                        .Build();


                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;


            _loggingModelBuilder
                        .SetSuccess()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{DeleteCustomerDemographic}")
                        .SetStatusCode(HttpStatusCode.OK.ToString())
                        .SetMethodType("DELETE")
                        .Build();


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id:int}/Customers")]
        public async Task<ActionResult<ApiResponse>> GetCustomersByTerritory(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("GET")
                        .SetErrorMessage("The model state is invalid!")
                        .Build();

                return BadRequest(_response);
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                        .SetMethodType("GET")
                        .SetErrorMessage("The given id is invalid!")
                        .Build();

                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.NotFound.ToString())
                        .SetMethodType("GET")
                        .SetErrorMessage("No customer demographic found with given id!")
                        .Build();

                return NotFound(_response);
            }

            var customers = await _customerDemographicRepository.GetCustomersByCustomerDemographicAsync(id);

            if(customers.Count() == 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No Record found!");


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.NotFound.ToString())
                        .SetMethodType("GET")
                        .SetErrorMessage("No record found")
                        .Build();

                return BadRequest(_response);
            }

            if(customers == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the customers");


                _loggingModelBuilder
                        .SetFailed()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                        .SetMethodType("GET")
                        .SetErrorMessage("Something went wrong while getting the customers")
                        .Build();

                return BadRequest(_response);
            }

            var customersResponse = _mapper.Map<List<ReadCustomerDto>>(customers);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customersResponse;


            _loggingModelBuilder
                        .SetSuccess()
                        .SetDetails($"{nameof(CustomerDemographicsController)}/{GetCustomersByTerritory}")
                        .SetStatusCode(HttpStatusCode.OK.ToString())
                        .SetMethodType("GET")
                        .Build();


            return Ok(_response);
        }
    }
}
