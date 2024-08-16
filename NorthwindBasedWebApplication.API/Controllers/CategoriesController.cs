using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.DTOs.CategoryDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.Common;
using AutoMapper;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;

namespace NorthwindBasedWebApplication.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.data = null;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var categories = await _categoryRepository.GetAllAsync(tracked: false);

            if (categories == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.data = null;
                _response.ErrorMessages.Add("No products found in database!");

                return BadRequest(_response);
            }

            var categoryDto = _mapper.Map<List<ReadCategoryDto>>(categories);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = categoryDto;


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
                _response.data = null;
                _response.IsSuccess = false;
            }

            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is invalid");
                _response.data = null;
                _response.IsSuccess = false;
            }

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer with given id is not found");
                _response.IsSuccess = false;
            }

            var category = await _categoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the category!");
                _response.data = null;
                _response.IsSuccess = false;
            }

            var categoryDto = _mapper.Map<ReadCategoryDto>(category);

            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages.Add(string.Empty);
            _response.data = categoryDto;
            _response.IsSuccess = true;

            return Ok(_response);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(CreateCategoryDto createCategoryDto)
        {

            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;



                return BadRequest(_response);
            }

            if (createCategoryDto == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while creating the category");
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            if (await _categoryRepository.IsExistAsync(cn => cn.CategoryName.Trim() == createCategoryDto.CategoryName.Trim()))
            {
                _response.ErrorMessages.Add("The name of category is exists, please choose another name");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            var categoryModel = _mapper.Map<Category>(createCategoryDto);


            var createdCategory = await _categoryRepository.CreateAsync(categoryModel);


            if (!createdCategory)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while creating the category");
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if (id <= 0 || updateCategoryDto.Id <= 0)
            {

                _response.ErrorMessages.Add("The given ids are not match!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                return BadRequest(_response);

            }

            if (id != updateCategoryDto.Id)
            {

                _response.ErrorMessages.Add("The given are not match!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }


            if (updateCategoryDto == null)
            {

                _response.ErrorMessages.Add("The data of category to update is empty!");
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.ErrorMessages.Add("The category with given id is not found!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }




            var updatedCategory = await _categoryRepository.UpdateAsync(_mapper.Map<Category>(updateCategoryDto));

            if (!updatedCategory)
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong while updating the category");


                return BadRequest(_response);
            }


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;


            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                return BadRequest(_response);
            }

            if (id <= 0)
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");


                return BadRequest(_response);
            }

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The category with given id not found");


                return BadRequest(_response);
            }

            var categoryModel = await _categoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (categoryModel == null)
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.ErrorMessages.Add("The content that you provided is empty");


                return BadRequest(_response);
            }

            var deletedCategory = await _categoryRepository.DeleteAsync(categoryModel);

            if (!deletedCategory)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while deleting the category");
                _response.StatusCode = HttpStatusCode.BadRequest;


                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;


            return Ok(_response);
        }



        [HttpGet]
        [Route("{id}/Products")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategory(int id)
        {

            if(id <= 0)
            {
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            if(!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The category with given id is not found!");

                return NotFound(_response);
            }


            var products = await _categoryRepository.GetProductsByCategory(id);

            if(products == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the products!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return NotFound(_response);
            }

            var productsResponse = _mapper.Map<List<ReadProductDto>>(products);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(string.Empty);
            _response.data = productsResponse;

            return NotFound(_response);
        }
    }
}
