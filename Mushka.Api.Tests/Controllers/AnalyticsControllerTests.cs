using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.Controllers;
using Mushka.Application.Analytics.Queries.GetBalance;
using Mushka.Application.Analytics.Queries.GetOrdersCount;
using Mushka.Application.Analytics.Queries.GetPopularCities;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Application.Analytics.Queries.GetSoldProductsCount;
using Mushka.Application.Analytics.Queries.GetUnpopularProducts;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class AnalyticsControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly AnalyticsController _controller;

    public AnalyticsControllerTests()
    {
        _controller = new AnalyticsController(_sender.Object);
    }

    [Fact]
    public async Task GetBalance_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new GetBalanceReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetBalanceQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetBalanceReadModel>(readModel));

        var result = await _controller.GetBalance(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetPopularProducts_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new PopularProductsReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetPopularProductsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<PopularProductsReadModel>(readModel));

        var result = await _controller.GetPopularProducts(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetUnpopularProducts_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new PopularProductsReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetUnpopularProductsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<PopularProductsReadModel>(readModel));

        var result = await _controller.GetUnpopularProducts(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetPopularCities_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new PopularCitiesReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetPopularCitiesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<PopularCitiesReadModel>(readModel));

        var result = await _controller.GetPopularCities(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetOrders_SendsQueryWithPeriod_AndReturnsReadModel()
    {
        var readModel = new OrdersCountReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<GetOrdersCountQuery>(query => query.Period == 6), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<OrdersCountReadModel>(readModel));

        var result = await _controller.GetOrders(6, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetSoldProducts_SendsQueryWithPeriod_AndReturnsReadModel()
    {
        var readModel = new SoldProductsCountReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<GetSoldProductsCountQuery>(query => query.Period == 12), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SoldProductsCountReadModel>(readModel));

        var result = await _controller.GetSoldProducts(12, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }
}
