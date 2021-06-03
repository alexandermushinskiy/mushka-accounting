using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
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
        
        [HttpGet("select")]
        public async Task<IActionResult> GetSelectProducts(bool inStock)
        {
            var products = await productService.GetInStockAsync(inStock, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Product>>, SelectProductsResponseModel>(products);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("~/api/v1/categories/{categoryId:guid}/products")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var products = await productService.GetByCategoryAsync(categoryId, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Product>>, ProductListResponseModel>(products);

            return actionResultProvider.Get(clientResponse);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await productService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Product>, ProductResponseModel>(product);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}/costprice")]
        public async Task<IActionResult> GetCostPrice(Guid id, int productsCount = 1)
        {
            var costPrice = await productService.GetCostPriceAsync(id, productsCount, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<ProductCostPrice>, CostPriceResponseModel>(costPrice);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProductRequestModel productRequest)
        {
            var product = mapper.Map<ProductRequestModel, Product>(productRequest);

            var productResponse = await productService.AddAsync(product, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]ProductRequestModel productRequest)
        {
            var product = mapper.Map<ProductRequestModel, Product>(productRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var productResponse = await productService.UpdateAsync(product, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productResponse = await productService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(productResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await productService.GetSizesAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Size>>, SizesResponseModel>(sizes);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] ExportRequestModel exportRequestModel)
        {
            var exportValidationResponse = await productService.ExportAsync(
                exportRequestModel.Title,
                exportRequestModel.ProductIds,
                cancellationTokenSourceProvider.Get().Token);

            return File(
                exportValidationResponse.Data.FileContent,
                exportValidationResponse.Data.ContentType,
                exportValidationResponse.Data.Name);
        }
    }
}