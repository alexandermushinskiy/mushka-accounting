using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Categories.Converters;
using Mushka.Application.Categories.Queries.Describe;
using Mushka.Application.Exceptions;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Categories.Queries.Describe;

public class DescribeCategoryQueryHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<ICategoryRepository> _repository = new Mock<ICategoryRepository>();
    private readonly Mock<IDescribeCategoryConverter> _converter = new Mock<IDescribeCategoryConverter>();
    private readonly DescribeCategoryQueryHandler _handler;

    public DescribeCategoryQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<ICategoryRepository>()).Returns(_repository.Object);
        _handler = new DescribeCategoryQueryHandler(_storage.Object, _converter.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeCategoryQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryNotFound);
    }

    [Fact]
    public async Task Handle_ReturnsConvertedReadModel()
    {
        var category = new Category { Id = Guid.NewGuid(), Name = "Socks" };
        var readModel = new DescribeCategoryReadModel();
        _repository
            .Setup(repository => repository.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _converter.Setup(converter => converter.ToReadModel(category)).Returns(readModel);

        var actual = await _handler.Handle(new DescribeCategoryQuery(category.Id), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
    }
}
