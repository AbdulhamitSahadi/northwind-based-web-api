using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.DTOs.CategoryDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.Common;
using AutoMapper;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;

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
        private readonly LoggingModelBuilder _loggingModelBuilder;




        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _response = new();
            _mapper = mapper;
            _logger = logger;
            _loggingModelBuilder = new();
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategories)}")
                    .SetFailed()
                    .SetErrorMessage("Model State is invalid!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetRole("Customer")
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

            var categories = await _categoryRepository.GetAllAsync(tracked: false);

            if(categories.Count == 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No category found!");

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategories)}")
                    .SetErrorMessage("Not category found!")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetRole("Customer")
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }


            if (categories == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while getting categories!");


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategories)}")
                    .SetErrorMessage("Something went wrong while getting categories!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetRole("Customer")
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

            var categoryDto = _mapper.Map<List<ReadCategoryDto>>(categories);

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.data = categoryDto;


            _loggingModelBuilder
                .SetMethodType("GET")
                .SetSuccess()
                .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategories)}")
                .SetStatusCode(HttpStatusCode.OK.ToString())
                .SetRole("Customer")
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
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetCategory(int id)
        {

            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The model state is invalid!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
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

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The customer with given id is not found");
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetErrorMessage("The customer with given id is not found")
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return NotFound(_response);
            }

            var category = await _categoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("En error exists while getting the category!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetErrorMessage("En error exists while getting the category!")
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

            var categoryDto = _mapper.Map<ReadCategoryDto>(category);

            _response.StatusCode = HttpStatusCode.OK;
            _response.data = categoryDto;
            _response.IsSuccess = true;


            _loggingModelBuilder
                .SetSuccess()
                .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                .SetMethodType("GET")
                .SetStatusCode(HttpStatusCode.OK.ToString())
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
        public async Task<ActionResult<ApiResponse>> CreateCategory(CreateCategoryDto createCategoryDto)
        {

            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("POST")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetErrorMessage("The model states are not valid!")
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


            if (createCategoryDto == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No content exists while sent the create data for category!");
                _response.StatusCode = HttpStatusCode.NoContent;

                _loggingModelBuilder
                    .SetStatusCode(HttpStatusCode.NoContent.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetFailed()
                    .SetErrorMessage("No content exists while sent the create data for category!")
                    .SetMethodType("POST")
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            if (await _categoryRepository.IsExistAsync(cn => cn.CategoryName.Trim() == createCategoryDto.CategoryName.Trim()))
            {
                _response.ErrorMessages.Add("The name of category is exists, please choose another name");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetErrorMessage("The name of category is exists, please choose another name!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetMethodType("POST")
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            var categoryModel = _mapper.Map<Category>(createCategoryDto);


            var createdCategory = await _categoryRepository.CreateAsync(categoryModel);


            if (!createdCategory)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while creating the category");
                _response.StatusCode = HttpStatusCode.BadRequest;


                _loggingModelBuilder
                    .SetErrorMessage("Something went wrong while creating the category!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetFailed()
                    .SetMethodType("POST")
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


            _loggingModelBuilder
                .SetStatusCode(HttpStatusCode.OK.ToString())
                .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                .SetMethodType("POST")
                .SetSuccess()
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
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetFailed()
                    .SetErrorMessage("The model states are not valid!")
                    .SetMethodType("PUT")
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

            if (id <= 0 || updateCategoryDto.Id <= 0)
            {

                _response.ErrorMessages.Add("The given id is not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;



                _loggingModelBuilder
                    .SetFailed()
                    .SetErrorMessage("The given id is not valid!")
                    .SetMethodType("PUT")
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);

            }

            if (id != updateCategoryDto.Id)
            {

                _response.ErrorMessages.Add("The given ids are not match!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetErrorMessage("The given ids are not match!")
                    .SetMethodType("PUT")
                    .SetFailed()
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (updateCategoryDto == null)
            {

                _response.ErrorMessages.Add("The data of category to update is empty!");
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetErrorMessage("The data of category to update is empty!")
                    .SetStatusCode(HttpStatusCode.NoContent.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetMethodType("PUT")
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.ErrorMessages.Add("The category with given id is not found!");
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetErrorMessage("The category with given id is not found!")
                    .SetStatusCode(HttpStatusCode.NotFound.ToString())
                    .SetMethodType("PUT")
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }




            var updatedCategory = await _categoryRepository.UpdateAsync(_mapper.Map<Category>(updateCategoryDto));

            if (!updatedCategory)
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong while updating the category");


                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetErrorMessage("Something went wrong while updating the category!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetMethodType("PUT")
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
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
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
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The model states are not valid!")
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

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The given id is not valid!");

                _loggingModelBuilder
                    .SetSuccess()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The given is is not valid!")
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            if (!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("The category with given id not found");

                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The category with given id is not found!")
                   .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            var categoryModel = await _categoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (categoryModel == null)
            {

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Something went wrong accessing the wanted category model");

                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong accessing the wanted category model!")
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

            var deletedCategory = await _categoryRepository.DeleteAsync(categoryModel);

            if (!deletedCategory)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Something went wrong while deleting the category");
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while deleting the category")
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


            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
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
        public async Task<ActionResult<ApiResponse>> GetProductsByCategory(int id)
        {

            if(id <= 0)
            {
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The given id is not valid!")
                   .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            if(!await _categoryRepository.IsExistAsync(i => i.Id == id, tracked: false))
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The category with given id is not found!");


                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The category with given id is not found!")
                   .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }


            var products = await _categoryRepository.GetProductsByCategory(id);

            if(products == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the products!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the products!")
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

            var productsResponse = _mapper.Map<List<ReadProductDto>>(products);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = false;
            _response.data = productsResponse;



            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
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

            return NotFound(_response);
        }
    }
}
