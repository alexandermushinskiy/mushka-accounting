using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
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

        [HttpGet("~/api/v1/categories/{categoryId:guid}/products")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var products = await productService.GetByCategoryAsync(categoryId, cancellationTokenSourceProvider.Get().Token);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProductRequestModel productRequest)
        {
            var product = mapper.Map<ProductRequestModel, Product>(productRequest);

            var productResponse = await productService.AddAsync(product, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Product>, ProductResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        //[HttpPut("{id:guid}")]
        //public async Task<IActionResult> Put(Guid id, [FromBody]ProductRequestModel productRequest)
        //{
        //    return await Put<ProductRequestModel, ProductResponseModel>(id, productRequest);
        //}

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productResponse = await productService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Product>, DeleteResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await productService.GetSizesAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Size>>, SizesResponseModel>(sizes);

            return actionResultProvider.Get(clientResponse);
        }

    }
}