using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.CorporateOrder;
using Mushka.Api.Converters.CorporateOrders;
using Mushka.Application.CorporateOrders.Commands;
using Mushka.Application.CorporateOrders.Queries.Describe;
using Mushka.Application.CorporateOrders.Queries.Search;
using Mushka.Application.CorporateOrders.Queries.ValidateNumber;

namespace Mushka.Api.Controllers;

[Route("api/v1/corporate-orders")]
public class CorporateOrdersController : ControllerBase
{
    private readonly ICorporateOrderRequestConverter _corporateOrderRequestConverter;

    public CorporateOrdersController(ISender sender, ICorporateOrderRequestConverter corporateOrderRequestConverter)
        : base(sender)
    {
        _corporateOrderRequestConverter = corporateOrderRequestConverter;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken)
    {
        var corporateOrders = await sender.Send(new SearchCorporateOrdersQuery(), cancellationToken);
        return Ok(corporateOrders);
    }

    [HttpGet("{id:guid}/describe")]
    public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
    {
        var corporateOrder = await sender.Send(new DescribeCorporateOrderQuery(id), cancellationToken);
        return Ok(corporateOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CorporateOrderRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_corporateOrderRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] CorporateOrderRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_corporateOrderRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCorporateOrderCommand(id), cancellationToken);
        return Ok();
    }

    [HttpPost("validate-number")]
    public async Task<IActionResult> ValidateOrderNumber(
        [FromBody] ValidateCorporateOrderNumberRequestModel request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ValidateCorporateOrderNumberQuery(request.OrderNumber), cancellationToken);
        return Ok(result);
    }
}
