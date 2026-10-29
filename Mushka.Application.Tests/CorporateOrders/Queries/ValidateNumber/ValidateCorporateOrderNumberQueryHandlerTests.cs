using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.CorporateOrders.Queries.ValidateNumber;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Queries.ValidateNumber;

public class ValidateCorporateOrderNumberQueryHandlerTests
{
    private readonly Mock<ICorporateOrderRepository> _repository = new Mock<ICorporateOrderRepository>();
    private readonly ValidateCorporateOrderNumberQueryHandler _handler;

    public ValidateCorporateOrderNumberQueryHandlerTests()
    {
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<ICorporateOrderRepository>()).Returns(_repository.Object);
        _repository
            .Setup(repository => repository.IsExistAsync(It.IsAny<Expression<Func<CorporateOrder, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<CorporateOrder, bool>> predicate, CancellationToken _) =>
                predicate.Compile()(new CorporateOrder { Number = "CO-1" }));
        _handler = new ValidateCorporateOrderNumberQueryHandler(storage.Object);
    }

    [Fact]
    public async Task Handle_WhenNumberExists_ReturnsInvalid()
    {
        var actual = await _handler.Handle(new ValidateCorporateOrderNumberQuery("CO-1"), CancellationToken.None);

        actual.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenNumberIsFree_ReturnsValid()
    {
        var actual = await _handler.Handle(new ValidateCorporateOrderNumberQuery("CO-2"), CancellationToken.None);

        actual.IsValid.Should().BeTrue();
    }
}
