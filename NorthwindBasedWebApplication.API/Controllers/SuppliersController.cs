using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<SuppliersController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public SuppliersController(ISupplierRepository supplierRepository, IMapper mapper,
            ILogger<SuppliersController> logger)
        {
            _supplierRepository = supplierRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = new();
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetSuppliers()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSuppliers)}")
                  .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                  .SetMethodType("GET")
                  .SetErrorMessage("The model state is invalid!")
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

            var suppliersModel = await _supplierRepository.GetAllAsync(tracked: false);

            if (suppliersModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting suppliers!");


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSuppliers)}")
                  .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                  .SetMethodType("GET")
                  .SetErrorMessage("Something went wrong while getting suppliers!")
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

            var supplierResponse = _mapper.Map<List<ReadSupplierDto>>(suppliersModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = supplierResponse;

            _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSuppliers)}")
                  .SetStatusCode(HttpStatusCode.OK.ToString())
                  .SetMethodType("GET")
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


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetSupplier(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("The given id is invalid")
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSupplier)}")
                 .SetStatusCode(HttpStatusCode.NotFound.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("No shipper found with given id!")
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

            var supplierModel = await _supplierRepository.GetAsync(i => i.Id == id, tracked: false);

            if (supplierModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the supplier!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSupplier)}")
                 .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("Something went wrong while getting the supplier!")
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

            var supplierResponse = _mapper.Map<ReadSupplierDto>(supplierModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = supplierResponse;
            _response.IsSuccess = true;

            _loggingModelBuilder
                 .SetSuccess()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetSupplier)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("GET")
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




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateSupplier([FromBody]CreateSupplierDto model)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(CreateSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("POST")
                 .SetErrorMessage("The model states is invalid!")
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

            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(CreateSupplier)}")
                 .SetStatusCode(HttpStatusCode.NoContent.ToString())
                 .SetMethodType("POST")
                 .SetErrorMessage("The content that send by user is empty!")
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

            if (await _supplierRepository.IsExistAsync(cn => cn.Phone.Trim() == model.Phone.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The shipper's phone is exists, please choose another!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(CreateSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("POST")
                 .SetErrorMessage("The shipper's phone is exists, please choose another!")
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

            var supplierModel = _mapper.Map<Supplier>(model);


            var createdSupplier = await _supplierRepository.CreateAsync(supplierModel);


            if (!createdSupplier)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while creating the supplier!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(CreateSupplier)}")
                 .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                 .SetMethodType("POST")
                 .SetErrorMessage("Something went wrong while creating the supplier!")
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
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(CreateSupplier)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("POST")
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
        public async Task<ActionResult<ApiResponse>> UpdateSupplier(int id, 
            [FromBody]UpdateSupplierDto updateSupplierDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("PUT")
                 .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0 || updateSupplierDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("PUT")
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

            if (id != updateSupplierDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("PUT")
                 .SetErrorMessage("No matching with given ids")
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


            if (updateSupplierDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.NoContent.ToString())
                 .SetMethodType("PUT")
                 .SetErrorMessage("The content of give model is empty")
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

            

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No supplier found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.NotFound.ToString())
                 .SetMethodType("PUT")
                 .SetErrorMessage("No supplier found with the given Id")
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

            var supplierModel = _mapper.Map<Supplier>(updateSupplierDto);


            var updatedSupplier = await _supplierRepository.UpdateAsync(supplierModel);

            if (!updatedSupplier)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went error while updating the supplier!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                 .SetMethodType("PUT")
                 .SetErrorMessage("Something went error while updating the supplier!")
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
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(UpdateSupplier)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("PUT")
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



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteSupplier(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("DELETE")
                 .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("DELETE")
                 .SetErrorMessage("The given id is not valid!")
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The supplier with given id is not found!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.NotFound.ToString())
                 .SetMethodType("DELETE")
                 .SetErrorMessage("The supplier with given id is not found!")
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

            var supplierModel = await _supplierRepository.GetAsync(i => i.Id == id, tracked: false);

            if (supplierModel == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the supplier model!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                 .SetMethodType("DELETE")
                 .SetErrorMessage("Something went wrong while getting the supplier model!")
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

            var deletedSupplier = await _supplierRepository.DeleteAsync(supplierModel);

            if (!deletedSupplier)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while deleting the supplier!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                 .SetMethodType("DELETE")
                 .SetErrorMessage("Something went wrong while deleting the supplier!")
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
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(DeleteSupplier)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("DELETE")
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


        [HttpGet]
        [Route("{id}/Products")]
        public async Task<ActionResult<ApiResponse>> GetProductsBySupplier(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetProductsBySupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("The model state is invalid!")
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

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;

                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetProductsBySupplier)}")
                 .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("The given id is invalid")
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

            if (!await _supplierRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetProductsBySupplier)}")
                 .SetStatusCode(HttpStatusCode.NotFound.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("No shipper found with given id!")
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

            var products = await _supplierRepository.GetProductsBySupplier(id);

            if(products == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the products of supplier");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetProductsBySupplier)}")
                 .SetStatusCode(HttpStatusCode.NotFound.ToString())
                 .SetMethodType("GET")
                 .SetErrorMessage("Something went wrong while getting the products of supplier")
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

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = _mapper.Map<List<ReadProductDto>>(products);

            _loggingModelBuilder
                 .SetSuccess()
                 .SetDetails($"{nameof(SuppliersController)}/{nameof(GetProductsBySupplier)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("GET")
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
    }
}
