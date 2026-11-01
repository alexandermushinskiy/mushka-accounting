using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Analytics.Converters;
using Mushka.Application.Analytics.Providers;
using Mushka.Application.Analytics.Queries.GetBalance;
using Mushka.Application.Analytics.Queries.GetOrdersCount;
using Mushka.Application.Analytics.Queries.GetPopularCities;
using Mushka.Application.Analytics.Queries.GetPopularProducts;
using Mushka.Application.Analytics.Queries.GetSoldProductsCount;
using Mushka.Application.Analytics.Queries.GetUnpopularProducts;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Analytics.Queries;

public class AnalyticsQueryHandlerTests
{
    private readonly Mock<IAnalyticsRepository> _repository = new Mock<IAnalyticsRepository>();
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new Mock<IDateTimeProvider>();

    public AnalyticsQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IAnalyticsRepository>()).Returns(_repository.Object);
    }

    [Fact]
    public async Task GetBalance_MapsExpenseAndProfit()
    {
        _repository
            .Setup(repository => repository.GetBalance(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Balance(120, 340));

        var actual = await new GetBalanceQueryHandler(_storage.Object)
            .Handle(new GetBalanceQuery(), CancellationToken.None);

        actual.Data.Expense.Should().Be(120);
        actual.Data.Profit.Should().Be(340);
    }

    [Fact]
    public async Task GetPopularProducts_RequestsTopSevenPopularProducts()
    {
        _repository
            .Setup(repository => repository.GetProductsByPopularity(7, Popularity.Popular, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new PopularProduct("Sock", "M", "S-1", 20) });

        var actual = await new GetPopularProductsQueryHandler(_storage.Object, new AnalyticConverter())
            .Handle(new GetPopularProductsQuery(), CancellationToken.None);

        actual.Data.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new PopularProductReadModel
            {
                Name = "Sock",
                SizeName = "M",
                VendorCode = "S-1",
                Quantity = 20
            });
    }

    [Fact]
    public async Task GetUnpopularProducts_RequestsTopSevenUnpopularProducts()
    {
        _repository
            .Setup(repository => repository.GetProductsByPopularity(7, Popularity.Unpopular, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PopularProduct>());

        await new GetUnpopularProductsQueryHandler(_storage.Object, new AnalyticConverter())
            .Handle(new GetUnpopularProductsQuery(), CancellationToken.None);

        _repository.Verify(
            repository => repository.GetProductsByPopularity(7, Popularity.Unpopular, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPopularCities_RequestsTopSevenCities()
    {
        _repository
            .Setup(repository => repository.GetPopularCities(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new PopularCity("Kyiv", 4) });

        var actual = await new GetPopularCitiesQueryHandler(_storage.Object)
            .Handle(new GetPopularCitiesQuery(), CancellationToken.None);

        actual.Data.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { City = "Kyiv", Quantity = 4 });
    }

    [Fact]
    public async Task GetOrdersCount_UsesFirstDayOfThePeriodWindow()
    {
        var now = new DateTime(2026, 11, 15);
        _dateTimeProvider.Setup(provider => provider.GetNow()).Returns(now);
        _repository
            .Setup(repository => repository.GetOrdersCount(new DateTime(2026, 9, 1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new OrdersCount(new DateTime(2026, 9, 1), 3) });

        var actual = await new GetOrdersCountQueryHandler(_storage.Object, _dateTimeProvider.Object, new AnalyticsPeriodProvider())
            .Handle(new GetOrdersCountQuery(3), CancellationToken.None);

        actual.Data.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { CreatedOn = new DateTime(2026, 9, 1), Quantity = 3 });
    }

    [Fact]
    public async Task GetSoldProductsCount_UsesFirstDayOfThePeriodWindow()
    {
        var now = new DateTime(2026, 11, 15);
        _dateTimeProvider.Setup(provider => provider.GetNow()).Returns(now);
        _repository
            .Setup(repository => repository.GetSoldProductsCount(new DateTime(2026, 11, 1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new SoldProductsCount(new DateTime(2026, 11, 1), 8) });

        var actual = await new GetSoldProductsCountQueryHandler(_storage.Object, _dateTimeProvider.Object, new AnalyticsPeriodProvider())
            .Handle(new GetSoldProductsCountQuery(1), CancellationToken.None);

        actual.Data.Should().ContainSingle()
            .Which.Quantity.Should().Be(8);
    }
}
