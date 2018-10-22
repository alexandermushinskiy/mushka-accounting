using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Service.Extensibility.Services;
using Mushka.Accounting.WebApi.ClientModels.Category;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>(categories);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await categoryService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Category>, CategoryResponseModel>(category);

            return actionResultProvider.Get(clientResponse);
        }

        //    [HttpPost]
        //    public async Task<IActionResult> Post([FromBody]CategoryRequestModel categoryRequest)
        //    {
        //        return await Post<CategoryRequestModel, CategoryResponseModel>(categoryRequest);
        //    }

        //    [HttpPut("{id:guid}")]
        //    public async Task<IActionResult> Put(Guid id, [FromBody]CategoryRequestModel categoryRequest)
        //    {
        //        return await Put<CategoryRequestModel, CategoryResponseModel>(id, categoryRequest);
        //    }

        //    [HttpDelete("{id:guid}")]
        //    public async Task<IActionResult> Delete(Guid id)
        //    {
        //        return await Delete<CategoryResponseModel>(id);
        //    }
    }
}