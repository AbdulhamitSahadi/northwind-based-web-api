using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using System.ComponentModel.Design.Serialization;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerDemographicsController : ControllerBase
    {
        private readonly ICustomerDemographicRepository _customerDemographicRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public CustomerDemographicsController(ICustomerDemographicRepository customerDemographicRepository, IMapper mapper)
        {
            _customerDemographicRepository = customerDemographicRepository;
            _response = new();
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographics()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var customerDemographics = await _customerDemographicRepository.GetAllAsync(tracked: false);

            if (customerDemographics == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No customer demographics found in database!");

                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<List<ReadCustomerDemographicDto>>(customerDemographics);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;


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

                return BadRequest(_response);
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;

                return NotFound(_response);
            }

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("En error exists while getting the customer demographic!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(customerDemographic);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;
            _response.IsSuccess = true;

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


                return BadRequest(_response);
            }

            if (createCustomerDemographicDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _customerDemographicRepository.IsExistAsync(cn => cn.CustomerDescription.Trim() == createCustomerDemographicDto.CustomerDescription.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The customer's description is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerDemographicModel = _mapper.Map<CustomerDemographic>(createCustomerDemographicDto);


            var createdCustomerDemographic = await _customerDemographicRepository.CreateAsync(customerDemographicModel);


            if (!createdCustomerDemographic)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while create customerDemographic.!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(createdCustomerDemographic);  

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, UpdateCustomerDemographicDto updateCustomerDemegraphicDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateCustomerDemegraphicDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateCustomerDemegraphicDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateCustomerDemegraphicDto == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with the given Id");
                _response.IsSuccess = false;


                return NotFound(_response);
            }

            var customerDemographicModel = _mapper.Map<CustomerDemographic>(updateCustomerDemegraphicDto);


            var updatedCustomerDemographic = await _customerDemographicRepository.UpdateAsync(customerDemographicModel);

            if (!updatedCustomerDemographic)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while updating the customer demographic!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(updatedCustomerDemographic);


            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicResponse;
            _response.IsSuccess = true;


            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer demographic with given id is not found!");
                _response.IsSuccess = false;


                return NotFound(_response);
            }

            var customerDemographic = await _customerDemographicRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerDemographic == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("the customer demographic model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedCustomerDemographic = await _customerDemographicRepository.DeleteAsync(customerDemographic);

            if (!deletedCustomerDemographic)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exist while deleting the customer demographic!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerDemographicResponse = _mapper.Map<ReadCustomerDemographicDto>(deletedCustomerDemographic);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = customerDemographicResponse;

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

                return BadRequest(_response);
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if (!await _customerDemographicRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer demographic found with given id!");
                _response.IsSuccess = false;

                return NotFound(_response);
            }

            var customers = await _customerDemographicRepository.GetCustomersByCustomerDemographicAsync(id);


            if(customers == null || customers.Count == 0)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went error while getting the customers of the demographic");

                return BadRequest(_response);
            }

            var customersResponse = _mapper.Map<List<ReadCustomerDto>>(customers);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customersResponse; 


            return Ok(_response);
        }
    }
}
