using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.RegionDTO;
using NorthwindBasedWebApplication.API.Models.DTOs.RegionDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _response = new();
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetRegions()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var regionsModel = await _regionRepository.GetAllAsync(tracked: false);

            if (regionsModel == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No regions found in database!");

                return BadRequest(_response);
            }

            var regionsResponse = _mapper.Map<List<ReadRegionDto>>(regionsModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = regionsResponse;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetRegion(int id)
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

            if (!await _regionRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No product found with given id!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var regionModel = await _regionRepository.GetAsync(i => i.Id == id, tracked: false);

            if (regionModel == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the region!");
                _response.IsSuccess = false;
            }

            var regionResponse = _mapper.Map<ReadRegionDto>(regionModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = regionResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody]CreateRegionDto model)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _regionRepository.IsExistAsync(cn => cn.RegionDescription.Trim() == model.RegionDescription.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The region's description is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var regionModel = _mapper.Map<Region>(model);


            var createdRegion = await _regionRepository.CreateAsync(regionModel);


            if (!createdRegion)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while creating the region!");


                return BadRequest(_response);
            } 

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, 
            [FromBody]UpdateRegionDto updateRegionDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateRegionDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateRegionDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateRegionDto == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _regionRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No region found with the given Id");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var regionModel = _mapper.Map<Region>(updateRegionDto);


            var updatedRegion = await _regionRepository.UpdateAsync(regionModel);

            if (!updatedRegion)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while updating the region!");
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
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _regionRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The product with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var regionModel = await _regionRepository.GetAsync(i => i.Id == id, tracked: false);

            if (regionModel == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("the region model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedRegion = await _regionRepository.DeleteAsync(regionModel);

            if (!deletedRegion)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exist while deleting the region!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Territories")]
        public async Task<ActionResult<ApiResponse>> GetTerritoriesByRegion(int id)
        {
            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _regionRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The product with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var territories = await _regionRepository.GetTerritoriesByRegionAsync(id);

            if(territories == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the territories");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return BadRequest(_response);      
            }

            var territoriesResponse = _mapper.Map<List<ReadTerritoryDto>>(territories);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = territoriesResponse;

            return Ok(_response);
        }
    }
}
