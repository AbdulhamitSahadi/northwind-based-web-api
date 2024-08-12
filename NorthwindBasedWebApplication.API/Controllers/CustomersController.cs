using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _response = new();
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCustomers()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var customers = await _customerRepository.GetAllAsync(tracked: false);

            if (customers == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No customers found in database!");

                return BadRequest(_response);
            }

            var customersResponse = _mapper.Map<List<ReadCustomerDto>>(customers);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customersResponse;

            return Ok(_response);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;
            }

            if (!await _customerRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer found with given id!");
                _response.IsSuccess = false;
            }

            var customer = await _customerRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customer == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the customer!");
                _response.IsSuccess = false;
            }

            var customerResponse = _mapper.Map<ReadCustomerDto>(customer);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(CreateCustomerDto createCustomerDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (createCustomerDto == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _customerRepository.IsExistAsync(cn => cn.ContactName.Trim() == createCustomerDto.ContactName.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The customer's contact name is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerModel = _mapper.Map<Customer>(createCustomerDto);


            var createdCustomer = await _customerRepository.CreateAsync(customerModel);


            if (!createdCustomer)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while creating the customer!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerResponse = _mapper.Map<ReadCustomerDto>(createdCustomer);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerResponse;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, UpdateCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateCustomerDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateCustomerDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateCustomerDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _customerRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No customer found with the given Id");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var ToUpdateCustomerModel = _mapper.Map<Customer>(updateCustomerDto);


            var updatedCustomer = await _customerRepository.UpdateAsync(ToUpdateCustomerModel);

            if (!updatedCustomer)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while updating the customer!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerResponse = _mapper.Map<ReadCustomerDto>(updatedCustomer);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerResponse;
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

            if (!await _customerRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var customerModel = await _customerRepository.GetAsync(i => i.Id == id, tracked: false);

            if (customerModel == null)
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("the customer model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedCustomer = await _customerRepository.DeleteAsync(customerModel);

            if (!deletedCustomer)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exist while deleting the customer!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("The customer deleted successfully!");
            _response.IsSuccess = false;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/CustomerDemographics")]
        public async Task<ActionResult<ApiResponse>> GetCustomerDemographicsByCustomer(int id)
        {

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _customerRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The customer with given id is not found");
                _response.StatusCode = HttpStatusCode.NotFound;


                return BadRequest(_response);
            }

            var customerDemographics = await _customerRepository.GetCustomerDemographicsByCustomerAsync(id);

            if(customerDemographics == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting customer demographics");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(_response);
            }

            var customerDemographicsResponse = _mapper.Map<List<ReadCustomerDemographicDto>>(customerDemographics);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = customerDemographicsResponse;

            return Ok(_response);
        }



        [HttpGet]
        [Route("{id}/Orders")]
        public async Task<ActionResult<ApiResponse>> GetOrdersByCustomer(int id)
        {
            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _customerRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.ErrorMessages.Add("The customer with given id is not found");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;


                return BadRequest(_response);
            }


            var orders = await _customerRepository.GetOrdersByCustomerAsync(id);

            if(orders == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting orders!");

                return BadRequest(_response);
            }

            var ordersResponse = _mapper.Map<List<ReadOrderDto>>(orders);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = ordersResponse;

            return Ok(ordersResponse);
        } 
    }
}
