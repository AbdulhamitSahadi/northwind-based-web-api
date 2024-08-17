
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;

namespace NorthwindBasedWebApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper, ILogger<OrdersController> logger,
            LoggingModelBuilder loggingModelBuilder)
        {
            _orderRepository = orderRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = loggingModelBuilder;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrders)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();

                return BadRequest(_response);
            }

            var ordersModel = await _orderRepository.GetAllAsync(tracked: false);

            if (ordersModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No orders found in database!");

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrders)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("No orders found in database!")
                   .Build();

                return BadRequest(_response);
            }

            var ordersResponse = _mapper.Map<List<ReadOrderDto>>(ordersModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = ordersResponse;



            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrders)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("GET")
                   .Build();


            return Ok(_response);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrder)}")
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
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The given id is invalid!")
                   .Build();

                return BadRequest(_response);   
            }

            if (!await _orderRepository.IsExistAsync(i => i.Id == id, tracked:false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No order found with given id!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrder)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("No order found with given id!")
                   .Build();

                return NotFound(_response);
            }

            var orderModel = await _orderRepository.GetAsync(i => i.Id == id, tracked: false);

            if (orderModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the order!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrder)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the order!")
                   .Build();

                return NotFound(_response);
            }

            var orderResponse = _mapper.Map<ReadOrderDto>(orderModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = orderResponse;
            _response.IsSuccess = true;

            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(GetOrder)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("GET")
                   .Build();

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromQuery]int employeeId,
            [FromQuery]int shipVia, [FromQuery]int customerId, 
            [FromBody]CreateOrderDto createOrderDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(CreateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();

                return BadRequest(_response);
            }

            if (createOrderDto == null)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(CreateOrder)}")
                   .SetStatusCode(HttpStatusCode.NoContent.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The content that send by the user is empty!")
                   .Build();


                return BadRequest(_response);
            }


            var orderModel = _mapper.Map<Order>(createOrderDto);

            orderModel.EmployeeId = employeeId;
            orderModel.ShipVia = shipVia;
            orderModel.CustomerId = customerId;


            var createdOrder = await _orderRepository.CreateAsync(orderModel);


            if (!createdOrder)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went error while creating the order");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(CreateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("Something went wrong while creating the order")
                   .Build();

                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(CreateOrder)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("POST")
                   .Build();

            return Ok(_response);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrder(int id,
            [FromQuery]int employeeId, [FromQuery]int shipVia,
            [FromQuery]int customerId, [FromBody]UpdateOrderDto updateOrderDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The model state is invalid!")
                   .Build();

                return BadRequest(_response);
            }

            if (id <= 0 || updateOrderDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The given id is invalid!")
                   .Build();

                return BadRequest(_response);

            }

            if (id != updateOrderDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("No matching with given ids")
                   .Build();

                return BadRequest(_response);
            }


            if (updateOrderDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.NoContent.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("The content of given model is empty")
                   .Build();

                return BadRequest(_response);
            }

            if (!await _orderRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No order found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("No order found with the given id!")
                   .Build();

                return NotFound(_response);
            }

            var orderModel = _mapper.Map<Order>(updateOrderDto);

            orderModel.EmployeeId = employeeId;
            orderModel.ShipVia = shipVia;
            orderModel.CustomerId = customerId;


            var updatedOrder = await _orderRepository.UpdateAsync(orderModel);

            if (!updatedOrder)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went error while updating the order");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("Something went wrong while updating the order")
                   .Build();



                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(UpdateOrder)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("PUT")
                   .Build();

            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteOrder(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
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
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The given id is not valid!")
                   .Build();

                return BadRequest(_response);
            }

            if (!await _orderRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer with given id is not found!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The customer with given id is not found!")
                   .Build();

                return NotFound(_response);
            }

            var orderModel = await _orderRepository.GetAsync(i => i.Id == id, tracked: false);

            if (orderModel == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the order!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while getting the order!")
                   .Build();

                return BadRequest(_response);
            }

            var deletedOrder = await _orderRepository.DeleteAsync(orderModel);

            if (!deletedOrder)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while deleting the order");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while deleting the order")
                   .Build();

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = false;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(OrdersController)}/{nameof(DeleteOrder)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("DELETE")
                   .Build();

            return Ok(_response);
        }
    }
}
