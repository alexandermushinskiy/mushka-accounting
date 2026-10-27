using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Exhibition;
using Mushka.Api.Converters.Exhibitions;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Application.Exhibitions.Queries.Describe;
using Mushka.Application.Exhibitions.Queries.GetDefaultProducts;
using Mushka.Application.Exhibitions.Queries.Search;

namespace Mushka.Api.Controllers;

[Route("api/v1/exhibitions")]
public class ExhibitionsController : ControllerBase
{
    private readonly IExhibitionRequestConverter _exhibitionRequestConverter;

    public ExhibitionsController(ISender sender, IExhibitionRequestConverter exhibitionRequestConverter)
        : base(sender)
    {
        _exhibitionRequestConverter = exhibitionRequestConverter;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken)
    {
        var exhibitions = await sender.Send(new SearchExhibitionsQuery(), cancellationToken);
        return Ok(exhibitions);
    }

    [HttpGet("{id:guid}/describe")]
    public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
    {
        var exhibition = await sender.Send(new DescribeExhibitionQuery(id), cancellationToken);
        return Ok(exhibition);
    }

    [HttpGet("default-products")]
    public async Task<IActionResult> GetDefaultProducts(CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetDefaultExhibitionProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ExhibitionRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_exhibitionRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] ExhibitionRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_exhibitionRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteExhibitionCommand(id), cancellationToken);
        return Ok();
    }
}
