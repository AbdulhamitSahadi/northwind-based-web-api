using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.EmployeeDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _response = new();
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEmployees()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var employees = await _employeeRepository.GetAllAsync(tracked: false);

            if (employees == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No employees found in database!");

                return BadRequest(_response);
            }

            var employeesResponse = _mapper.Map<List<ReadEmployeeDto>>(employees);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = employeesResponse;

            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEmployee(int id)
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

            if (!await _employeeRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No employee found with given id!");
                _response.IsSuccess = false;
            }

            var employeeModel = await _employeeRepository.GetAsync(i => i.Id == id, tracked: false);

            if (employeeModel == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the employee!");
                _response.IsSuccess = false;
            }

            var employeeResponse = _mapper.Map<ReadEmployeeDto>(employeeModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages.Add(string.Empty);
            _response.data = employeeResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromQuery] int? reportsTo,
            [FromBody] CreateEmployeeDto createEmployeeDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (createEmployeeDto == null)
            {
                _response.ErrorMessages.Add("Something went wrong while creating the employee");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            if (await _employeeRepository.IsExistAsync(cn => cn.HomePhone.Trim() == createEmployeeDto.HomePhone.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The customer's Home phone is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var employeeModel = _mapper.Map<Employee>(createEmployeeDto);

            employeeModel.ReportsTo = reportsTo;


            var createdEmployee = await _employeeRepository.CreateAsync(employeeModel);


            if (!createdEmployee)
            {
                _response.ErrorMessages.Add("Something went error while creating employee");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromQuery] int? reportsTo,
            [FromBody]UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateEmployeeDto.Id <= 0)
            {

                _response.ErrorMessages.Add("The given ids are not match!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);

            }

            if (id != updateEmployeeDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateEmployeeDto == null)
            {

                _response.ErrorMessages.Add("The content you provide is null");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NoContent;


                return BadRequest(_response);
            }

            if (!await _employeeRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The employee with given id is not found");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var employeeModel = _mapper.Map<Employee>(updateEmployeeDto);

            employeeModel.ReportsTo = reportsTo;

            var updatedEmployee = await _employeeRepository.UpdateAsync(employeeModel);

            if (!updatedEmployee)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while updating the employee!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states is invalid!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;



                return BadRequest(_response);
            }

            if (id <= 0)
            {

                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            if (!await _employeeRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The employee with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var employeeModel = await _employeeRepository.GetAsync(i => i.Id == id, tracked: false);

            if (employeeModel == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("the employee model is null!");
 
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedEmployee = await _employeeRepository.DeleteAsync(employeeModel);

            if (!deletedEmployee)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exist while deleting the employee!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Territories")]
        public async Task<ActionResult<ApiResponse>> GetTerritoriesByEmployee(int id)
        {
            if(id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }

            if(!await _employeeRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The employee with given id is not found");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var territories = await _employeeRepository.GetTerritoriesByEmployeeAsync(id);

            if(territories == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the territories");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }

            var territoriesResponse = _mapper.Map<List<ReadTerritoryDto>>(territories);

            _response.data = territoriesResponse;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Orders")]
        public async Task<ActionResult<ApiResponse>> GetOrdersByEmployee(int id)
        {
            if(id <= 0)
            {
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);

            }

            if(!await _employeeRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The employee with given id not found");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var orders = await _employeeRepository.GetOrdersByEmployeeAsync(id);

            if(orders == null && orders.Count <= 0)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the orders");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);   
            }

            var ordersResponse = _mapper.Map<List<ReadOrderDto>>(orders);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = ordersResponse;

            return Ok(_response);
        }
    }
}
