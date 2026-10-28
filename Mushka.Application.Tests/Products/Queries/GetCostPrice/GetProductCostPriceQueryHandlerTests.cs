using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Products.Queries.GetCostPrice;
using Mushka.Application.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.GetCostPrice;

public class GetProductCostPriceQueryHandlerTests
{
    private readonly Mock<IProductRepository> _repository = new Mock<IProductRepository>();
    private readonly Mock<ICostPriceProvider> _costPriceProvider = new Mock<ICostPriceProvider>();
    private readonly GetProductCostPriceQueryHandler _handler;

    public GetProductCostPriceQueryHandlerTests()
    {
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(_repository.Object);
        _handler = new GetProductCostPriceQueryHandler(storage.Object, _costPriceProvider.Object);
    }

    [Fact]
    public async Task Handle_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new GetProductCostPriceQuery(id, 2), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _costPriceProvider.Verify(
            provider => provider.CalculateAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsCalculatedCostPrice()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = id });
        _costPriceProvider
            .Setup(provider => provider.CalculateAsync(id, 3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(4.5m);

        var actual = await _handler.Handle(new GetProductCostPriceQuery(id, 3), CancellationToken.None);

        actual.CostPrice.Should().Be(4.5m);
    }
}
