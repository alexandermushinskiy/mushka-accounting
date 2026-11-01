using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Orders.Queries.ValidateNumber;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Orders.Queries.ValidateNumber;

public class ValidateOrderNumberQueryHandlerTests
{
    private readonly ValidateOrderNumberQueryHandler _handler;

    public ValidateOrderNumberQueryHandlerTests()
    {
        var repository = new Mock<IOrderRepository>();
        repository
            .Setup(mock => mock.IsExistAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Order, bool>> predicate, CancellationToken _) =>
                predicate.Compile()(new Order { Number = "O-1" }));
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IOrderRepository>()).Returns(repository.Object);
        _handler = new ValidateOrderNumberQueryHandler(storage.Object);
    }

    [Fact]
    public async Task Handle_WhenNumberExists_ReturnsInvalid()
    {
        var actual = await _handler.Handle(new ValidateOrderNumberQuery("O-1"), CancellationToken.None);

        actual.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenNumberIsFree_ReturnsValid()
    {
        var actual = await _handler.Handle(new ValidateOrderNumberQuery("O-2"), CancellationToken.None);

        actual.IsValid.Should().BeTrue();
    }
}
