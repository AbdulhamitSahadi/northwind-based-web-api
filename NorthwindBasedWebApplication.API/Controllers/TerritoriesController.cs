using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using Microsoft.OpenApi.Validations;
using NorthwindBasedWebApplication.API.Models.DTOs.EmployeeDTOs;
using System.Security.Cryptography;
using NorthwindBasedWebApplication.API.Models.DTOs.RegionDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TerritoriesController : ControllerBase
    {
        private readonly ITerritoryRepository _territoryRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public TerritoriesController(ITerritoryRepository territoryRepository, IMapper mapper)
        {
            _territoryRepository = territoryRepository;
            _response = new();
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetTerritories()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var territoriesModel = await _territoryRepository.GetAllAsync(tracked: false);

            if (territoriesModel == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No territories found in database!");

                return BadRequest(_response);
            }

            var territoriesResponse = _mapper.Map<List<ReadTerritoryDto>>(territoriesModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = territoriesResponse;
            _response.ErrorMessages.Add(string.Empty);


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetTerritory(int id)
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

            if (!await _territoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var territoryModel = await _territoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (territoryModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("En error exists while getting the territory!");
                _response.IsSuccess = false;
            }

            var territoryResponse = _mapper.Map<ReadTerritoryDto>(territoryModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages.Add(string.Empty);
            _response.data = territoryResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(
            [FromQuery] int regionId, [FromBody] CreateTerritoryDto createTerritoryDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (createTerritoryDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _territoryRepository.IsExistAsync(cn => cn.TerritoryDescription.Trim() == createTerritoryDto.TerritoryDescription.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The territory's description is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var territoryModel = _mapper.Map<Territory>(createTerritoryDto);

            territoryModel.RegionId = regionId;


            var createdTerritory = await _territoryRepository.CreateAsync(territoryModel);


            if (!createdTerritory)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exists while creating the territory!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id,
            [FromQuery] int regionId, [FromBody] UpdateTerritoryDto updateTerritoryDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateTerritoryDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateTerritoryDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateTerritoryDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _territoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No territory found with the given Id");
                _response.IsSuccess = false;


                return NotFound(_response);
            }

            var territoryModel = _mapper.Map<Territory>(updateTerritoryDto);

            territoryModel.RegionId = regionId;


            var updatedTerritory = await _territoryRepository.UpdateAsync(territoryModel);

            if (!updatedTerritory)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exists while updating the supplier!");
                _response.data = null;
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

            if (!await _territoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The territory with given id is not found!");
                _response.IsSuccess = false;


                return NotFound(_response);
            }

            var territoryModel = await _territoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (territoryModel == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("the territory model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedTerritory = await _territoryRepository.DeleteAsync(territoryModel);

            if (!deletedTerritory)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exist while deleting the territory!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Employees")]
        public async Task<ActionResult<ApiResponse>> GetEmployeesByTerritory(int id)
        {
            if(id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if(!await _territoryRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.ErrorMessages.Add("The territory with given id is not found!");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(_response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employees = await _territoryRepository.GetEmployeesByTerritoryAsync(id);

            if(employees == null || employees.Count <= 0)
            {
                _response.ErrorMessages.Add("Something went error while getting the employees of the territory");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return BadRequest(_response);
            }

            var employeesResponse = _mapper.Map<List<ReadEmployeeDto>>(employees);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = employeesResponse;

            return Ok(_response);
        }


        [HttpGet]
        [Route("{id:int}/Region")]
        public async Task<ActionResult<ApiResponse>> GetRegionByTerritory(int id)
        {
            if(id <= 0)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }

            if(!await _territoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The territory with given id is not found!");

                return NotFound(_response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(_response);
            }

            var region = await _territoryRepository.GetRegionByTerritoryAsync(id);

            if(region == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the region of the territory!");

                return BadRequest(_response);
            }

            var regionResponse = _mapper.Map<ReadRegionDto>(region);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = regionResponse;

            return Ok(_response);
        }
    }
}
