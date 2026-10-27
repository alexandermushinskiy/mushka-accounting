using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Application.Exhibitions.Queries.GetDefaultProducts;
using Mushka.Application.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Queries.GetDefaultProducts;

public class GetDefaultExhibitionProductsQueryHandlerTests
{
    private static readonly Guid PostcardId = Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5");
    private static readonly Guid PackageId = Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7");

    [Fact]
    public async Task Handle_ReturnsDefaultProductsInStock_WithCostPriceCalculatedFromStockQuantity()
    {
        var postcard = new Product
        {
            Id = PostcardId,
            Name = "Postcard",
            VendorCode = "PC-1",
            Quantity = 4,
            Size = new Size { Name = "One" }
        };
        var package = new Product { Id = PackageId, Name = "Package", Quantity = 0 };
        var other = new Product { Id = Guid.NewGuid(), Name = "Socks", Quantity = 9 };
        var products = new[] { postcard, package, other };

        var productRepository = new Mock<IProductRepository>();
        productRepository
            .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Product, bool>> predicate, CancellationToken _) =>
                products.Where(predicate.Compile()).ToList());
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(productRepository.Object);

        var costPriceProvider = new Mock<ICostPriceProvider>();
        costPriceProvider
            .Setup(provider => provider.CalculateAsync(PostcardId, 4, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2.5m);

        var handler = new GetDefaultExhibitionProductsQueryHandler(
            storage.Object,
            costPriceProvider.Object,
            new GetDefaultExhibitionProductsConverter());

        var actual = await handler.Handle(new GetDefaultExhibitionProductsQuery(), CancellationToken.None);

        var product = actual.Products.Should().ContainSingle().Subject;
        product.Id.Should().Be(PostcardId);
        product.Name.Should().Be("Postcard");
        product.VendorCode.Should().Be("PC-1");
        product.SizeName.Should().Be("One");
        product.Quantity.Should().Be(1);
        product.UnitPrice.Should().Be(0);
        product.CostPrice.Should().Be(2.5m);
        costPriceProvider.Verify(
            provider => provider.CalculateAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
