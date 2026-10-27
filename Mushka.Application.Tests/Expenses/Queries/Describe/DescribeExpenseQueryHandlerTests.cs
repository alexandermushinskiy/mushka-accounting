using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Expenses.Converters;
using Mushka.Application.Expenses.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Queries.Describe;

public class DescribeExpenseQueryHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IExpenseRepository> _repository = new Mock<IExpenseRepository>();
    private readonly Mock<IDescribeExpenseConverter> _converter = new Mock<IDescribeExpenseConverter>();
    private readonly DescribeExpenseQueryHandler _handler;

    public DescribeExpenseQueryHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IExpenseRepository>()).Returns(_repository.Object);
        _handler = new DescribeExpenseQueryHandler(_storage.Object, _converter.Object);
    }

    [Fact]
    public async Task Handle_WhenExpenseIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeExpenseQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExpenseNotFound);
    }

    [Fact]
    public async Task Handle_ReturnsConvertedReadModel()
    {
        var expense = new Expense { Id = Guid.NewGuid(), Purpose = "Ads" };
        var readModel = new DescribeExpenseReadModel();
        _repository
            .Setup(repository => repository.GetByIdAsync(expense.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expense);
        _converter.Setup(converter => converter.ToReadModel(expense)).Returns(readModel);

        var actual = await _handler.Handle(new DescribeExpenseQuery(expense.Id), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
    }
}
