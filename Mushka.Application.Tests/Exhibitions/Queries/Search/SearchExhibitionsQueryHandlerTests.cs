using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Application.Exhibitions.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Queries.Search;

public class SearchExhibitionsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsExhibitionsInRepositoryOrder()
    {
        var exhibitions = new[]
        {
            new Exhibition
            {
                Name = "Beta",
                Products = new List<ExhibitionProduct> { new ExhibitionProduct { Quantity = 2 } }
            },
            new Exhibition
            {
                Name = "Alpha",
                Products = new List<ExhibitionProduct> { new ExhibitionProduct { Quantity = 1 } }
            }
        };
        var repository = new Mock<IExhibitionRepository>();
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(exhibitions);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IExhibitionRepository>()).Returns(repository.Object);
        var handler = new SearchExhibitionsQueryHandler(storage.Object, new SearchExhibitionsConverter());

        var actual = await handler.Handle(new SearchExhibitionsQuery(), CancellationToken.None);

        actual.Total.Should().Be(2);
        actual.Items.Select(item => item.Name).Should().Equal("Beta", "Alpha");
        actual.Items.Select(item => item.ProductsCount).Should().Equal(2, 1);
    }
}
