using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Converters;
using Mushka.Application.Products.Queries.Select;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.Select;

public class SelectProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenInStock_ReturnsProductsWithQuantity_OrderedByName()
    {
        var inStock = new Product { Name = "Beta", Quantity = 2, Category = new Category { Name = "Clothes" } };
        var outOfStock = new Product { Name = "Alpha", Quantity = 0, Category = new Category { Name = "Clothes" } };
        var products = new[] { inStock, outOfStock };
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Product, bool>> predicate, CancellationToken _) =>
                products.Where(predicate.Compile()).ToList());
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(repository.Object);
        var handler = new SelectProductsQueryHandler(storage.Object, new SelectProductsConverter());

        var actual = await handler.Handle(new SelectProductsQuery(true), CancellationToken.None);

        actual.Items.Select(item => item.Name).Should().Equal("Beta");
        repository.Verify(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNotInStock_ReturnsAllProductsOrderedByName()
    {
        var products = new[]
        {
            new Product { Name = "Beta", Category = new Category { Name = "Clothes" } },
            new Product { Name = "Alpha", Category = new Category { Name = "Clothes" } }
        };
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(repository.Object);
        var handler = new SelectProductsQueryHandler(storage.Object, new SelectProductsConverter());

        var actual = await handler.Handle(new SelectProductsQuery(false), CancellationToken.None);

        actual.Items.Select(item => item.Name).Should().Equal("Alpha", "Beta");
    }
}
