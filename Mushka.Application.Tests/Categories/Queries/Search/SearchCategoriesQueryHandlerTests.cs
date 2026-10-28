using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Categories.Converters;
using Mushka.Application.Categories.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Categories.Queries.Search;

public class SearchCategoriesQueryHandlerTests
{
    [Fact]
    public async Task Handle_OrdersCategoriesByOrderDescending_AndReturnsReadModel()
    {
        var categories = new[]
        {
            new Category { Name = "Low", Order = 1 },
            new Category { Name = "High", Order = 5 }
        };
        var repository = new Mock<ICategoryRepository>();
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<ICategoryRepository>()).Returns(repository.Object);
        var handler = new SearchCategoriesQueryHandler(storage.Object, new SearchCategoriesConverter());

        var actual = await handler.Handle(new SearchCategoriesQuery(), CancellationToken.None);

        actual.Total.Should().Be(2);
        actual.Items.Select(item => item.Name).Should().Equal("High", "Low");
    }
}
