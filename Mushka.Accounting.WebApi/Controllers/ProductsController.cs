using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Entities;
using Mushka.Accounting.Service.Extensibility.Services;
using Mushka.Accounting.WebApi.ClientModels.Product;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            IProductService productService)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Product>>, ProductsResponseModel>(products);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await productService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Product>, ProductResponseModel>(product);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost("~/api/v1/categories/{categoryId:guid}/products")]
        public async Task<IActionResult> Post(Guid categoryId, [FromBody]ProductRequestModel productRequest)
        {
            var product = mapper.Map<ProductRequestModel, Product>(productRequest);
            product.CategoryId = categoryId;

            var productResponse = await productService.AddAsync(product, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Product>, ProductResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        //[HttpPut("{id:guid}")]
        //public async Task<IActionResult> Put(Guid id, [FromBody]ProductRequestModel productRequest)
        //{
        //    return await Put<ProductRequestModel, ProductResponseModel>(id, productRequest);
        //}

        //[HttpDelete("{id:guid}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    return await Delete<ProductResponseModel>(id);
        //}
    }
}