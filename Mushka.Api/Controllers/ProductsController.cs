using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Product;
using Mushka.Api.Converters.Products;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Queries.Describe;
using Mushka.Application.Products.Queries.Export;
using Mushka.Application.Products.Queries.GetByCategory;
using Mushka.Application.Products.Queries.GetCostPrice;
using Mushka.Application.Products.Queries.GetSizes;
using Mushka.Application.Products.Queries.Select;

namespace Mushka.Api.Controllers;

[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRequestConverter _productRequestConverter;

    public ProductsController(ISender sender, IProductRequestConverter productRequestConverter)
        : base(sender)
    {
        _productRequestConverter = productRequestConverter;
    }

    [HttpGet("select")]
    public async Task<IActionResult> Select(bool inStock, CancellationToken cancellationToken)
    {
        var products = await sender.Send(new SelectProductsQuery(inStock), cancellationToken);
        return Ok(products);
    }

    [HttpGet("~/api/v1/categories/{categoryId:guid}/products")]
    public async Task<IActionResult> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetProductsByCategoryQuery(categoryId), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await sender.Send(new DescribeProductQuery(id), cancellationToken);
        return Ok(product);
    }

    [HttpGet("{id:guid}/costprice")]
    public async Task<IActionResult> GetCostPrice(Guid id, int productsCount = 1, CancellationToken cancellationToken = default)
    {
        var costPrice = await sender.Send(new GetProductCostPriceQuery(id, productsCount), cancellationToken);
        return Ok(costPrice);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_productRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProductRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_productRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteProductCommand(id), cancellationToken);
        return Ok();
    }

    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizes(CancellationToken cancellationToken)
    {
        var sizes = await sender.Send(new GetProductSizesQuery(), cancellationToken);
        return Ok(sizes);
    }

    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] ExportProductsRequestModel request, CancellationToken cancellationToken)
    {
        var file = await sender.Send(new ExportProductsQuery(request.Title, request.ProductIds?.ToList()), cancellationToken);
        return File(file.Content, file.ContentType, file.Name);
    }
}
