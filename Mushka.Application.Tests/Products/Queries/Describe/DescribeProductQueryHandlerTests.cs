using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Products.Converters;
using Mushka.Application.Products.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.Describe;

public class DescribeProductQueryHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IProductRepository> _repository = new Mock<IProductRepository>();
    private readonly Mock<IDescribeProductConverter> _converter = new Mock<IDescribeProductConverter>();
    private readonly DescribeProductQueryHandler _handler;

    public DescribeProductQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_repository.Object);
        _handler = new DescribeProductQueryHandler(_storage.Object, _converter.Object);
    }

    [Fact]
    public async Task Handle_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeProductQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
    }

    [Fact]
    public async Task Handle_ReturnsConvertedReadModel()
    {
        var product = new Product { Id = Guid.NewGuid(), Name = "Socks" };
        var readModel = new DescribeProductReadModel();
        _repository
            .Setup(repository => repository.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _converter.Setup(converter => converter.ToReadModel(product)).Returns(readModel);

        var actual = await _handler.Handle(new DescribeProductQuery(product.Id), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
    }
}
