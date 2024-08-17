using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.DTOs.CategoryDTOs;
using System.IO.Pipelines;
using NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs;
using NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        private readonly LoggingModelBuilder _loggingModelBuilder;

        public ProductsController(IProductRepository productRepository, IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = new();
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetProducts()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProducts)}")
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

            var productsModel = await _productRepository.GetAllAsync(tracked: false);

            if (productsModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting the products");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProducts)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the products")
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

            var productsResponse = _mapper.Map<List<ReadProductDto>>(productsModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = productsResponse;

            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProducts)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetProduct(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProduct)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The model states is invalid")
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
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProduct)}")
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

            if (!await _productRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No product found with given id!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("No product found with given id!")
                   .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }

            var productModel = await _productRepository.GetAsync(i => i.Id == id, tracked: false);

            if (productModel == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the product!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the product")
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

            var productResponse = _mapper.Map<ReadProductDto>(productModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = productResponse;
            _response.IsSuccess = true;


            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetProduct)}")
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
        public async Task<ActionResult<ApiResponse>> CreateProduct([FromQuery]int categoryId,
            [FromQuery]int supplierId, [FromBody]CreateProductDto createProductDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(CreateProduct)}")
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

            if (createProductDto == null)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(CreateProduct)}")
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

            if (await _productRepository.IsExistAsync(cn => cn.ProductName.Trim() == createProductDto.ProductName.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The product name is exists, please choose another!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(CreateProduct)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("The product name is exists, please choose another!")
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

            var productModel = _mapper.Map<Product>(createProductDto);

            productModel.CategoryId = categoryId;
            productModel.SupplierId = supplierId;


            var createdProduct = await _productRepository.CreateAsync(productModel);


            if (!createdProduct)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while creating the product!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(CreateProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("POST")
                   .SetErrorMessage("Something went wrong while creating the product!")
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
                   .SetSuccess()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(CreateProduct)}")
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
        public async Task<ActionResult<ApiResponse>> UpdateProduct(int id, 
            [FromQuery]int categoryId, [FromQuery] int supplierId, UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
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

            if (id <= 0 || updateProductDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
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

            if (id != updateProductDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
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


            if (updateProductDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
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

            if (!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No product found with the given Id");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("No product found with the given Id")
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return NotFound(_response);
            }

            var productModel = _mapper.Map<Product>(updateProductDto);

            productModel.SupplierId = supplierId;
            productModel.CategoryId = categoryId;


            var updatedProduct = await _productRepository.UpdateAsync(productModel);

            if (!updatedProduct)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while updating the product");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("PUT")
                   .SetErrorMessage("Something went wrong while updating the product")
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
                   .SetSuccess()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(UpdateProduct)}")
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
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
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
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
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

            if (!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The product with given id is not found!")
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

            var productModel = await _productRepository.GetAsync(i => i.Id == id, tracked: false);

            if (productModel == null)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the product!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while getting the product!")
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

            var deletedProduct = await _productRepository.DeleteAsync(productModel);

            if (!deletedProduct)
            {

                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the product!");
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while getting the product!")
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
                   .SetDetails($"{nameof(ProductsController)}/{nameof(DeleteProduct)}")
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
        [Route("{id}/Category")]
        public async Task<ActionResult<ApiResponse>> GetCategoryByProduct(int id)
        {
            if(id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The given id is invalid!");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetCategoryByProduct)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
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

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetCategoryByProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The product with given id is not found!")
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }

            var category = await _productRepository.GetCategoryByProductAsync(id);

            if(category == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the category");

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetCategoryByProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the category")
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

            var categoryResponse = _mapper.Map<ReadCategoryDto>(category);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = categoryResponse;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetCategoryByProduct)}")
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
        [Route("{id}/Supplier")]
        public async Task<ActionResult<ApiResponse>> GetSupplierByProduct(int id)
        {
            if(id <= 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetSupplierByProduct)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
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

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetSupplierByProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The product with given id is not found!")
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }

            var supplier = await _productRepository.GetSupplierByProductAsync(id);

            if(supplier == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong while getting the supplier of the product");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetSupplierByProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the supplier of the product")
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

            var supplierResponse = _mapper.Map<ReadSupplierDto>(supplier);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = supplierResponse;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetSupplierByProduct)}")
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
        [Route("{id}/Orders")]
        public async Task<ActionResult<ApiResponse>> GetOrdersByProduct(int id)
        {
            if(id <= 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetOrdersByProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
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

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");

                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetOrdersByProduct)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The product with given id is not found!")
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }

            var orders = await _productRepository.GetOrdersByProductAsync(id);

            if(orders == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while getting the orders of this products!");


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetOrdersByProduct)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the orders of this products!")
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

            var ordersResponse = _mapper.Map<List<ReadOrderDto>>(orders);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = ordersResponse;


            _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(ProductsController)}/{nameof(GetOrdersByProduct)}")
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
