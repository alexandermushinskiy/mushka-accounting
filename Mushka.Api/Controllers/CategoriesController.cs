using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Category;
using Mushka.Api.Converters.Categories;
using Mushka.Application.Categories.Commands;
using Mushka.Application.Categories.Queries.Describe;
using Mushka.Application.Categories.Queries.Search;

namespace Mushka.Api.Controllers;

[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRequestConverter _categoryRequestConverter;

    public CategoriesController(ISender sender, ICategoryRequestConverter categoryRequestConverter)
        : base(sender)
    {
        _categoryRequestConverter = categoryRequestConverter;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken)
    {
        var categories = await sender.Send(new SearchCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}/describe")]
    public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
    {
        var category = await sender.Send(new DescribeCategoryQuery(id), cancellationToken);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCategoryRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_categoryRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCategoryRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_categoryRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCategoryCommand(id), cancellationToken);
        return Ok();
    }
}
