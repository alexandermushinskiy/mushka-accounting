using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Application.Exhibitions.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Queries.Describe;

public class DescribeExhibitionQueryHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IExhibitionRepository> _repository = new Mock<IExhibitionRepository>();
    private readonly Mock<IDescribeExhibitionConverter> _converter = new Mock<IDescribeExhibitionConverter>();
    private readonly DescribeExhibitionQueryHandler _handler;

    public DescribeExhibitionQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IExhibitionRepository>()).Returns(_repository.Object);
        _handler = new DescribeExhibitionQueryHandler(_storage.Object, _converter.Object);
    }

    [Fact]
    public async Task Handle_WhenExhibitionIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Exhibition)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeExhibitionQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExhibitionNotFound);
    }

    [Fact]
    public async Task Handle_ReturnsConvertedReadModel()
    {
        var exhibition = new Exhibition { Id = Guid.NewGuid(), Name = "Fair" };
        var readModel = new DescribeExhibitionReadModel();
        _repository
            .Setup(repository => repository.GetByIdAsync(exhibition.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exhibition);
        _converter.Setup(converter => converter.ToReadModel(exhibition)).Returns(readModel);

        var actual = await _handler.Handle(new DescribeExhibitionQuery(exhibition.Id), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
    }
}
