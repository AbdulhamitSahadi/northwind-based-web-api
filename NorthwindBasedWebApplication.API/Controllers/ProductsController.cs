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

        public ProductsController(IProductRepository productRepository, IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetProducts()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var productsModel = await _productRepository.GetAllAsync(tracked: false);

            if (productsModel == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No products found in database!");

                return BadRequest(_response);
            }

            var productsResponse = _mapper.Map<List<ReadProductDto>>(productsModel);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = productsResponse;


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

                return BadRequest(_response);
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if (!await _productRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No product found with given id!");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var productModel = await _productRepository.GetAsync(i => i.Id == id, tracked: false);

            if (productModel == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the product!");
                _response.IsSuccess = false;
            }

            var productResponse = _mapper.Map<ReadProductDto>(productModel);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = productResponse;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromQuery]int categoryId,
            [FromQuery]int supplierId, [FromBody]CreateProductDto createProductDto)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (createProductDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The content that send by user is empty!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (await _productRepository.IsExistAsync(cn => cn.ProductName.Trim() == createProductDto.ProductName.Trim()))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The product name is exists, please choose another!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var productModel = _mapper.Map<Product>(createProductDto);

            productModel.CategoryId = categoryId;
            productModel.SupplierId = supplierId;


            var createdProduct = await _productRepository.CreateAsync(productModel);


            if (!createdProduct)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while creating the product!");
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
            [FromQuery]int categoryId, [FromQuery] int supplierId, UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0 || updateProductDto.Id <= 0)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid!");
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateProductDto.Id)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No matching with given ids");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateProductDto == null)
            {

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content of give model is empty");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("No product found with the given Id");
                _response.IsSuccess = false;


                return NotFound(_response);
            }

            var productModel = _mapper.Map<Product>(updateProductDto);

            productModel.SupplierId = supplierId;
            productModel.CategoryId = categoryId;


            var updatedProduct = await _productRepository.UpdateAsync(productModel);

            if (!updatedProduct)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exists while updating the product!");
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

            if (!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var productModel = await _productRepository.GetAsync(i => i.Id == id, tracked: false);

            if (productModel == null)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("the product model is null!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var deletedProduct = await _productRepository.DeleteAsync(productModel);

            if (!deletedProduct)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error exist while deleting the product!");
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


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

                return BadRequest(_response);
            }

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");

                return NotFound(_response);
            }

            var category = await _productRepository.GetCategoryByProductAsync(id);

            if(category == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong while getting the category");

                return BadRequest(_response);
            }

            var categoryResponse = _mapper.Map<ReadCategoryDto>(category);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = categoryResponse;

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

                return BadRequest(_response);   
            }

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");

                return NotFound(_response);
            }

            var supplier = await _productRepository.GetSupplierByProductAsync(id);

            if(supplier == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong while getting the supplier of the product");

                return BadRequest(_response);
            }

            var supplierResponse = _mapper.Map<ReadSupplierDto>(supplier);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = supplierResponse;

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

                return BadRequest(_response);
            }

            if(!await _productRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The product with given id is not found!");

                return NotFound(_response);
            }

            var orders = await _productRepository.GetOrdersByProductAsync(id);

            if(orders == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong while getting the orders of this products!");

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
