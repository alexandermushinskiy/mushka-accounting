using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Converters;
using Mushka.Application.Products.Queries.GetSizes;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.GetSizes;

public class GetProductSizesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsSizesInRepositoryOrder()
    {
        var sizes = new[]
        {
            new Size { Name = "L" },
            new Size { Name = "M" }
        };
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(mock => mock.GetSizesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sizes);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(repository.Object);
        var handler = new GetProductSizesQueryHandler(storage.Object, new GetProductSizesConverter());

        var actual = await handler.Handle(new GetProductSizesQuery(), CancellationToken.None);

        actual.Items.Select(item => item.Name).Should().Equal("L", "M");
    }
}
