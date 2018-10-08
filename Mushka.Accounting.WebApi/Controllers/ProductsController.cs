using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Service.Extensibility.Services;
using Mushka.Accounting.WebApi.ClientModels.Requests;
using Mushka.Accounting.WebApi.ClientModels.Responses;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase<Product>
    {
        public ProductsController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            IServiceBase<Product> serviceBase)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper, serviceBase)
        {
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await GetById<ProductResponseModel>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(  [FromBody]ProductRequestModel productRequest)
        {
            return await Post<ProductRequestModel, ProductResponseModel>(productRequest);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]ProductRequestModel productRequest)
        {
            return await Put<ProductRequestModel, ProductResponseModel>(id, productRequest);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await Delete<ProductResponseModel>(id);
        }
    }
}