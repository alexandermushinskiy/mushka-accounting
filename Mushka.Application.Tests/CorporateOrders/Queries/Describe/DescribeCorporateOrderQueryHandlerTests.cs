using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.CorporateOrders.Queries.Describe;
using Mushka.Application.Exceptions;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Queries.Describe;

public class DescribeCorporateOrderQueryHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<ICorporateOrderRepository> _repository = new Mock<ICorporateOrderRepository>();
    private readonly Mock<IDescribeCorporateOrderConverter> _converter = new Mock<IDescribeCorporateOrderConverter>();
    private readonly DescribeCorporateOrderQueryHandler _handler;

    public DescribeCorporateOrderQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<ICorporateOrderRepository>()).Returns(_repository.Object);
        _handler = new DescribeCorporateOrderQueryHandler(_storage.Object, _converter.Object);
    }

    [Fact]
    public async Task Handle_WhenOrderIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CorporateOrder)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeCorporateOrderQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CorporateOrderNotFound);
    }

    [Fact]
    public async Task Handle_ReturnsConvertedReadModel()
    {
        var order = new CorporateOrder { Id = Guid.NewGuid(), Number = "CO-1" };
        var readModel = new DescribeCorporateOrderReadModel();
        _repository
            .Setup(repository => repository.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _converter.Setup(converter => converter.ToReadModel(order)).Returns(readModel);

        var actual = await _handler.Handle(new DescribeCorporateOrderQuery(order.Id), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
    }
}
