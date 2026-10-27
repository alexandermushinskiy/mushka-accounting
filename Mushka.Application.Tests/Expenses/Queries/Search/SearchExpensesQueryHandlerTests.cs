using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Expenses.Converters;
using Mushka.Application.Expenses.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Queries.Search;

public class SearchExpensesQueryHandlerTests
{
    [Fact]
    public async Task Handle_OrdersExpensesByCreatedOn_AndReturnsReadModel()
    {
        var expenses = new[]
        {
            new Expense { Purpose = "Later", CreatedOn = new DateTime(2024, 2, 2) },
            new Expense { Purpose = "Earlier", CreatedOn = new DateTime(2024, 1, 1) }
        };
        var repository = new Mock<IExpenseRepository>();
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expenses);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IExpenseRepository>()).Returns(repository.Object);
        var handler = new SearchExpensesQueryHandler(storage.Object, new SearchExpensesConverter());

        var actual = await handler.Handle(new SearchExpensesQuery(), CancellationToken.None);

        actual.Total.Should().Be(2);
        actual.Items.Should().SatisfyRespectively(
            first => first.Purpose.Should().Be("Earlier"),
            second => second.Purpose.Should().Be("Later"));
    }
}
