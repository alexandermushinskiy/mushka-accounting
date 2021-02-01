using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Category;
using Mushka.WebApi.ClientModels.Category.GetById;
using Mushka.WebApi.ClientModels.Category.Search;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            ICategoryService categoryService)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.categoryService = categoryService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            var categories = await categoryService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Category>>, SearchCategoriesResponseModel>(categories);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await categoryService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Category>, CategoryResponseModel>(category);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryRequestModel categoryRequest)
        {
            var category = mapper.Map<CategoryRequestModel, Category>(categoryRequest);

            var categoryResponse = await categoryService.AddAsync(category, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Category>, EmptyResponseModel>(categoryResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CategoryRequestModel categoryRequest)
        {
            var category = mapper.Map<CategoryRequestModel, Category>(categoryRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var categoryResponse = await categoryService.UpdateAsync(category, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Category>, EmptyResponseModel>(categoryResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryResponse = await categoryService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Category>, DeleteResponseModel>(categoryResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}