using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Converters;
using Mushka.Application.Products.Queries.GetByCategory;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.GetByCategory;

public class GetProductsByCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_OrdersProductsByName()
    {
        var categoryId = System.Guid.NewGuid();
        var products = new[]
        {
            new Product { Name = "Beta", Supplies = new List<SupplyProduct>() },
            new Product { Name = "Alpha", Supplies = new List<SupplyProduct>() }
        };
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(mock => mock.GetByCategoryId(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(repository.Object);
        var handler = new GetProductsByCategoryQueryHandler(storage.Object, new GetProductsByCategoryConverter());

        var actual = await handler.Handle(new GetProductsByCategoryQuery(categoryId), CancellationToken.None);

        actual.Items.Select(item => item.Name).Should().Equal("Alpha", "Beta");
    }
}
