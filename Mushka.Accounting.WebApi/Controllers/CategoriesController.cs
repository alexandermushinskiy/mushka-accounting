using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Service.Extensibility.Services;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase<Category>
    {
        public CategoriesController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            ICategoryService categoryService)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper, categoryService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await GetAll<CategoriesResponseModel>();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await GetById<CategoryResponseModel>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CategoryRequestModel categoryRequest)
        {
            return await Post<CategoryRequestModel, CategoryResponseModel>(categoryRequest);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]CategoryRequestModel categoryRequest)
        {
            return await Put<CategoryRequestModel, CategoryResponseModel>(id, categoryRequest);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await Delete<CategoryResponseModel>(id);
        }
    }
}