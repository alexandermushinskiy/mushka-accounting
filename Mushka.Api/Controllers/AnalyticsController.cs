using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Application.Analytics.Queries.GetBalance;
using Mushka.Application.Analytics.Queries.GetOrdersCount;
using Mushka.Application.Analytics.Queries.GetPopularCities;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Application.Analytics.Queries.GetSoldProductsCount;
using Mushka.Application.Analytics.Queries.GetUnpopularProducts;

namespace Mushka.Api.Controllers;

[Route("api/v1/analytics")]
public class AnalyticsController : ControllerBase
{
    public AnalyticsController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        var balance = await sender.Send(new GetBalanceQuery(), cancellationToken);
        return Ok(balance);
    }

    [HttpGet("popular-products")]
    public async Task<IActionResult> GetPopularProducts(CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetPopularProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("unpopular-products")]
    public async Task<IActionResult> GetUnpopularProducts(CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetUnpopularProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("popular-cities")]
    public async Task<IActionResult> GetPopularCities(CancellationToken cancellationToken)
    {
        var cities = await sender.Send(new GetPopularCitiesQuery(), cancellationToken);
        return Ok(cities);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(int period, CancellationToken cancellationToken)
    {
        var orders = await sender.Send(new GetOrdersCountQuery(period), cancellationToken);
        return Ok(orders);
    }

    [HttpGet("sold-products")]
    public async Task<IActionResult> GetSoldProducts(int period, CancellationToken cancellationToken)
    {
        var soldProducts = await sender.Send(new GetSoldProductsCountQuery(period), cancellationToken);
        return Ok(soldProducts);
    }
}
