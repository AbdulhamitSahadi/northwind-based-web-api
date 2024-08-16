using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(ISupplierRepository supplierRepository, IMapper mapper,
            ILogger<SuppliersController> logger)
        {
            _supplierRepository = supplierRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetSuppliers()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var suppliersModel = await _supplierRepository.GetAllAsync(tracked: false);

            if (suppliersModel == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No suppliers found in database!");

                return BadRequest(_response);
            }

            var supplierResponse = _mapper.Map<List<ReadSupplierDto>>(suppliersModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = supplierResponse;
            _response.ErrorMessages.Add(string.Empty);


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetSupplier(int id)
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var supplierModel = await _supplierRepository.GetAsync(i => i.Id == id, tracked: false);

            if (supplierModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("En error exists while getting the supplier!");
                _response.IsSuccess = false;
            }

            var supplierResponse = _mapper.Map<ReadSupplierDto>(supplierModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages.Add(string.Empty);
            _response.data = supplierResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody]CreateSupplierDto model)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.data = null;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _supplierRepository.IsExistAsync(cn => cn.Phone.Trim() == model.Phone.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The shipper's phone is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var supplierModel = _mapper.Map<Supplier>(model);


            var createdSupplier = await _supplierRepository.CreateAsync(supplierModel);


            if (!createdSupplier)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exists while creating the supplier!");
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
            [FromBody]UpdateSupplierDto updateSupplierDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateSupplierDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateSupplierDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateSupplierDto == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No supplier found with the given Id");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var supplierModel = _mapper.Map<Supplier>(updateSupplierDto);


            var updatedSupplier = await _supplierRepository.UpdateAsync(supplierModel);

            if (!updatedSupplier)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exists while updating the supplier!");
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The supplier with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var supplierModel = await _supplierRepository.GetAsync(i => i.Id == id, tracked: false);

            if (supplierModel == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("the supplier model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedSupplier = await _supplierRepository.DeleteAsync(supplierModel);

            if (!deletedSupplier)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error exist while deleting the supplier!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Products")]
        public async Task<ActionResult<ApiResponse>> GetProductsBySupplier(int id)
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var products = await _supplierRepository.GetProductsBySupplier(id);

            if(products == null || products.Count <= 0)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the products of supplier");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = _mapper.Map<List<ReadProductDto>>(products);

            return Ok(_response);
        }
    }
}
