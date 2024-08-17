using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.ShipperDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippersController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<ShippersController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public ShippersController(IShipperRepository shipperRepository, IMapper mapper,
            ILogger<ShippersController> logger)
        {
            _shipperRepository = shipperRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = new();
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShippers()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShippers)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();

                return BadRequest(_response);
            }

            var shippersModel = await _shipperRepository.GetAllAsync(tracked: false);

            if (shippersModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the shippers!");

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShippers)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the shippers!")
                   .Build();

                return BadRequest(_response);
            }

            var shippersResponse = _mapper.Map<List<ReadShipperDto>>(shippersModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = shippersResponse;

            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShippers)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("GET")
                   .Build();


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetShipper(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShipper)}")
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
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The given id is invalid")
                   .Build();

                return BadRequest(_response);
            }

            if (!await _shipperRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No shipper found with given id!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShipper)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("No shipper found with given id!")
                   .Build();

                return BadRequest(_response);
            }

            var shipperModel = await _shipperRepository.GetAsync(i => i.Id == id, tracked: false);

            if (shipperModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the shipper!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShipper)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the shipper!")
                   .Build();


                return BadRequest(_response);
            }

            var shipperResponse = _mapper.Map<ReadShipperDto>(shipperModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = shipperResponse;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetShipper)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("GET")
                   .Build();

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateShipper([FromBody]CreateShipperDto createShipperDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(CreateShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();


                return BadRequest(_response);
            }

            if (createShipperDto == null)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(CreateShipper)}")
                   .SetStatusCode(HttpStatusCode.NoContent.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The content that send by user is empty!")
                   .Build();

                return BadRequest(_response);
            }

            if (await _shipperRepository.IsExistAsync(cn => cn.Phone.Trim() == createShipperDto.Phone.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The shipper's phone is exists, please choose another!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(CreateShipper)}")
                   .SetStatusCode(HttpStatusCode.NoContent.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The shipper's phone is exists, please choose another!")
                   .Build();

                return BadRequest(_response);
            }

            var shipperModel = _mapper.Map<Shipper>(createShipperDto);


            var createdShipper = await _shipperRepository.CreateAsync(shipperModel);


            if (!createdShipper)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while creating the shipper!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(CreateShipper)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("Something went wrong while creating the shipper!")
                   .Build();

                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(CreateShipper)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("POST")
                   .Build();

            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateShipper(int id, 
            [FromBody]UpdateShipperDto updateShipperDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();


                return BadRequest(_response);
            }

            if (id <= 0 || updateShipperDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The given id is invalid!")
                   .Build();

                return BadRequest(_response);

            }

            if (id != updateShipperDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("No matching with given ids")
                   .Build();

                return BadRequest(_response);
            }


            if (updateShipperDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.NoContent.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The content of give model is empty")
                   .Build();

                return BadRequest(_response);
            }

            if (!await _shipperRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No shipper found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("No shipper found with the given Id")
                   .Build();

                return BadRequest(_response);
            }

            var shipperModel = _mapper.Map<Shipper>(updateShipperDto);


            var updatedShipper = await _shipperRepository.UpdateAsync(shipperModel);

            if (!updatedShipper)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while updating the shipper!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("Something went wrong while updating the shipper!")
                   .Build();

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(UpdateShipper)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("PUT")
                   .Build();

            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteShipper(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
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
                   .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The given id is not valid!")
                   .Build();

                return BadRequest(_response);
            }

            if (!await _shipperRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The shipper with given id is not found!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
                  .SetStatusCode(HttpStatusCode.NotFound.ToString())
                  .SetMethodType("DELETE")
                  .SetErrorMessage("The shipper with given id is not found!")
                  .Build();

                return NotFound(_response);
            }

            var shipperModel = await _shipperRepository.GetAsync(i => i.Id == id, tracked: false);

            if (shipperModel == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the shipper!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
                  .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                  .SetMethodType("DELETE")
                  .SetErrorMessage("Something went wrong while getting the shipper!")
                  .Build();


                return BadRequest(_response);
            }

            var deletedShipper = await _shipperRepository.DeleteAsync(shipperModel);

            if (!deletedShipper)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while deleting the shipper!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
                  .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                  .SetMethodType("DELETE")
                  .SetErrorMessage("Something went wrong while deleting the shipper!")
                  .Build();

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;


            _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(DeleteShipper)}")
                  .SetStatusCode(HttpStatusCode.OK.ToString())
                  .SetMethodType("DELETE")
                  .Build();

            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}/Orders")]
        public async Task<ActionResult<ApiResponse>> GetOrdersByShipper(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(GetOrdersByShipper)}")
                  .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                  .SetMethodType("GET")
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
                  .SetDetails($"{nameof(ShippersController)}/{nameof(GetOrdersByShipper)}")
                  .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                  .SetMethodType("GET")
                  .SetErrorMessage("The given id is not valid!")
                  .Build();

                return BadRequest(_response);
            }

            if (!await _shipperRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The shipper with given id is not found!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(GetOrdersByShipper)}")
                  .SetStatusCode(HttpStatusCode.NotFound.ToString())
                  .SetMethodType("GET")
                  .SetErrorMessage("The shipper with given id is not found!")
                  .Build();

                return NotFound(_response);
            }


            var orders = await _shipperRepository.GetOrdersByShipper(id);

            if(orders == null)
            {
                _response.ErrorMessages.Add("Something went wrong wile getting the orders of the shipper");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                  .SetFailed()
                  .SetDetails($"{nameof(ShippersController)}/{nameof(GetOrdersByShipper)}")
                  .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                  .SetMethodType("GET")
                  .SetErrorMessage("Something went wrong wile getting the orders of the shipper")
                  .Build();

                return BadRequest(_response);
            }


            var ordersResponse = _mapper.Map<List<ReadOrderDto>>(orders);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = ordersResponse;

            _loggingModelBuilder
                 .SetFailed()
                 .SetDetails($"{nameof(ShippersController)}/{nameof(GetOrdersByShipper)}")
                 .SetStatusCode(HttpStatusCode.OK.ToString())
                 .SetMethodType("GET")
                 .Build();

            return Ok(_response);
        }
    }
}
