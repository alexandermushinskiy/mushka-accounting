using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Providers;

public class CostPriceProviderTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
    private readonly Mock<ISupplyRepository> _supplyRepository = new Mock<ISupplyRepository>();
    private readonly CostPriceProvider _provider;

    public CostPriceProviderTests()
    {
        _provider = new CostPriceProvider(_supplyRepository.Object, _orderRepository.Object);
    }

    [Fact]
    public async Task CalculateAsync_WhenNothingIsSold_UsesFirstSupplyCostPrice()
    {
        var productId = Guid.NewGuid();
        Setup(productId, 0, new[] { Supply(productId, 10, 1.2m) });

        var actual = await _provider.CalculateAsync(productId, 1);

        actual.Should().Be(1.2m);
    }

    [Fact]
    public async Task CalculateAsync_WhenFirstSupplyIsSoldOut_UsesNextSupplyCostPrice()
    {
        var productId = Guid.NewGuid();
        Setup(productId, 10, new[]
        {
            Supply(productId, 10, 1.2m),
            Supply(productId, 13, 1.5m)
        });

        var actual = await _provider.CalculateAsync(productId, 1);

        actual.Should().Be(1.5m);
    }

    [Fact]
    public async Task CalculateAsync_WhenQuantitySpansSupplies_AveragesCostPrices()
    {
        var productId = Guid.NewGuid();
        Setup(productId, 8, new[]
        {
            Supply(productId, 10, 1.2m),
            Supply(productId, 13, 2.5m)
        });

        var actual = await _provider.CalculateAsync(productId, 4);

        actual.Should().Be(1.85m);
    }

    [Fact]
    public async Task CalculateAsync_IgnoresSuppliesForOtherProducts()
    {
        var productId = Guid.NewGuid();
        Setup(productId, 2, new[]
        {
            Supply(productId, 10, 1.2m),
            Supply(Guid.NewGuid(), 13, 12.7m)
        });

        var actual = await _provider.CalculateAsync(productId, 4);

        actual.Should().Be(1.2m);
    }

    private void Setup(Guid productId, int soldCount, IEnumerable<SupplyProduct> supplies)
    {
        _orderRepository
            .Setup(repository => repository.GetSoldProductCount(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(soldCount);
        _supplyRepository
            .Setup(repository => repository.GetByProductAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplies);
    }

    private static SupplyProduct Supply(Guid productId, int quantity, decimal costPrice) =>
        new SupplyProduct
        {
            ProductId = productId,
            Quantity = quantity,
            CostPrice = costPrice
        };
}
