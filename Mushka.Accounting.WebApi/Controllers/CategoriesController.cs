using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Service.Extensibility;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;
using Mushka.Accounting.WebApi.Filters;

namespace Mushka.Accounting.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [TypeFilter(typeof(ModelStateValidationFilterAttribute))]
    public class CategoriesController : Controller
    {
        private readonly ICancellationTokenSourceProvider cancellationTokenSourceProvider;
        private readonly IMapper mapper;
        private readonly IActionResultProvider actionResultProvider;
        private readonly ICategoryService categoryService;

        public CategoriesController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IMapper mapper,
            ICategoryService categoryService,
            IActionResultProvider actionResultProvider)
        {
            this.cancellationTokenSourceProvider = cancellationTokenSourceProvider;
            this.mapper = mapper;
            this.categoryService = categoryService;
            this.actionResultProvider = actionResultProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ValidationResponse<IEnumerable<Category>> categories = await categoryService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            CategoriesResponseModel clientResponse = mapper.Map<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>(categories);

            return Ok(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ValidationResponse<Category> category = await categoryService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            CategoryResponseModel clientResponse = mapper.Map<ValidationResponse<Category>, CategoryResponseModel>(category);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostCategoryRequestModel categoryRequest)
        {
            ValidationResponse<Category> response = await categoryService.AddAsync(
                mapper.Map<PostCategoryRequestModel, Category>(categoryRequest),
                cancellationTokenSourceProvider.Get().Token);

            CategoryResponseModel clientResponse = mapper.Map<ValidationResponse<Category>, CategoryResponseModel>(response);

            return actionResultProvider.Get(clientResponse, StatusCodes.Status201Created);
        }

    }
}