using Microsoft.AspNetCore.Mvc;
using NorthwindBasedWebApplication.API.Models.DTOs.CategoryDTOs;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;
using System.Net;
using NorthwindBasedWebApplication.API.Models.Common;
using AutoMapper;
using NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin,Customer")]
        
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {

            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                .SetRole(roleClaims.First().Value.ToString())
                .SetUser(user.Identity.Name.ToString())
                .Build();

            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


            



            return Ok(_response);
        }


        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ApiResponse>> GetCategory(int id)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();

            ClaimsPrincipal user = this.User;

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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetErrorMessage("The given id is invalid!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }

            var category = await _categoryRepository.GetAsync(i => i.Id == id, tracked: false);

            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("En error exists while getting the category!");
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetMethodType("GET")
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(GetCategory)}")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetErrorMessage("En error exists while getting the category!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                .SetRole(roleClaims.First().Value.ToString())
                .SetUser(user.Identity.Name.ToString())
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                _response.StatusCode = HttpStatusCode.BadRequest;

                _loggingModelBuilder
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetFailed()
                    .SetErrorMessage("No content exists while sent the create data for category!")
                    .SetMethodType("POST")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }

            if (await _categoryRepository.IsExistAsync(cn => cn.CategoryName.Trim() == createCategoryDto.CategoryName.Trim()))
            {
                _response.ErrorMessages.Add("The name of category is exists, please choose another name!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                    .SetFailed()
                    .SetErrorMessage("The name of category is exists, please choose another name!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetMethodType("POST")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                _response.StatusCode = HttpStatusCode.InternalServerError;


                _loggingModelBuilder
                    .SetErrorMessage("Something went wrong while creating the category!")
                    .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(CreateCategory)}")
                    .SetFailed()
                    .SetMethodType("POST")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                .SetRole(roleClaims.First().Value.ToString())
                .SetUser(user.Identity.Name.ToString())
                .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

            return Ok(_response);
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {

            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();

                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


                return BadRequest(_response);
            }


            if (updateCategoryDto == null)
            {

                _response.ErrorMessages.Add("The data of category to update is empty!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetErrorMessage("The data of category to update is empty!")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(UpdateCategory)}")
                    .SetMethodType("PUT")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

            return Ok(_response);
        }



        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
        {

            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (!ModelState.IsValid)
            {
                _response.ErrorMessages.Add("The model states are not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _loggingModelBuilder
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The model states are not valid!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                    .SetFailed()
                    .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                    .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                    .SetMethodType("DELETE")
                    .SetErrorMessage("The given is is not valid!")
                    .SetRole(roleClaims.First().Value.ToString())
                    .SetUser(user.Identity.Name.ToString())
                    .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("The category with given id is not found!")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong accessing the wanted category model!")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(DeleteCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("DELETE")
                   .SetErrorMessage("Something went wrong while deleting the category")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);


            return Ok(_response);
        }



        [HttpGet]
        [Route("{id}/Products")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<ApiResponse>> GetProductsByCategory(int id)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role.ToString()).ToList();
            ClaimsPrincipal user = this.User;

            if (id <= 0)
            {
                _response.ErrorMessages.Add("The given id is not valid!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.BadRequest.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The given id is not valid!")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogWarning("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
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
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.NotFound.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("The category with given id is not found!")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return BadRequest(_response);
            }


            var products = await _categoryRepository.GetProductsByCategory(id);

            if(products == null)
            {
                _response.ErrorMessages.Add("Something went wrong while getting the products!");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;


                _loggingModelBuilder
                   .SetFailed()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.InternalServerError.ToString())
                   .SetMethodType("GET")
                   .SetErrorMessage("Something went wrong while getting the products!")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();


                _logger.LogError("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

                return NotFound(_response);
            }

            var productsResponse = _mapper.Map<List<ReadProductDto>>(products);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.data = productsResponse;



            _loggingModelBuilder
                   .SetSuccess()
                   .SetDetails($"{nameof(CategoriesController)}/{nameof(GetProductsByCategory)}")
                   .SetStatusCode(HttpStatusCode.OK.ToString())
                   .SetMethodType("GET")
                   .SetRole(roleClaims.First().Value.ToString())
                   .SetUser(user.Identity.Name.ToString())
                   .Build();



            _logger.LogInformation("{Details}|{StatusCode}|{MethodType}|{User}|{Role}|{Success}{Failed}|{ErrorMessage}",
                    _loggingModelBuilder.Build().Details,
                    _loggingModelBuilder.Build().StatusCode,
                    _loggingModelBuilder.Build().MethodType,
                    _loggingModelBuilder.Build().User,
                    _loggingModelBuilder.Build().Role,
                    _loggingModelBuilder.Build().Success,
                    _loggingModelBuilder.Build().Failed,
                    _loggingModelBuilder.Build().ErrorMessage);

            return Ok(_response);
        }
    }
}
