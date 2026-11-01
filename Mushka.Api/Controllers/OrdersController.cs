using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Order;
using Mushka.Api.Converters.Orders;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Queries.Describe;
using Mushka.Application.Orders.Queries.GetDefaultProducts;
using Mushka.Application.Orders.Queries.Search;
using Mushka.Application.Orders.Queries.ValidateNumber;

namespace Mushka.Api.Controllers;

[Route("api/v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRequestConverter _orderRequestConverter;

    public OrdersController(ISender sender, IOrderRequestConverter orderRequestConverter)
        : base(sender)
    {
        _orderRequestConverter = orderRequestConverter;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchOrdersRequestModel request, CancellationToken cancellationToken)
    {
        var orders = await sender.Send(_orderRequestConverter.ToSearchQuery(request), cancellationToken);
        return Ok(orders);
    }

    [HttpGet("default-products")]
    public async Task<IActionResult> GetDefaultProducts(CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetDefaultOrderProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}/describe")]
    public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
    {
        var order = await sender.Send(new DescribeOrderQuery(id), cancellationToken);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_orderRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] OrderRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_orderRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteOrderCommand(id), cancellationToken);
        return Ok();
    }

    [HttpPost("validate-number")]
    public async Task<IActionResult> ValidateOrderNumber(
        [FromBody] ValidateOrderNumberRequestModel request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ValidateOrderNumberQuery(request.OrderNumber), cancellationToken);
        return Ok(result);
    }
}
