using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Expense;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Expenses;
using Mushka.Application.Expenses.Commands;
using Mushka.Application.Expenses.Queries.Describe;
using Mushka.Application.Expenses.Queries.Search;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class ExpensesControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<IExpenseRequestConverter> _converter = new Mock<IExpenseRequestConverter>();
    private readonly ExpensesController _controller;

    public ExpensesControllerTests()
    {
        _controller = new ExpensesController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SearchExpensesReadModel { Total = 1 };
        _sender
            .Setup(sender => sender.Send(It.IsAny<SearchExpensesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchExpensesReadModel>(readModel));

        var result = await _controller.Search(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeExpenseReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeExpenseQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeExpenseReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new CreateExpenseRequestModel();
        var command = new CreateExpenseCommand(ExpenseModel());
        _converter.Setup(converter => converter.ToCreateCommand(request)).Returns(command);
        _sender
            .Setup(sender => sender.Send(command, It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Post(request, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _sender.Verify(sender => sender.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Put_SendsConvertedCommand_AndReturnsOk()
    {
        var id = Guid.NewGuid();
        var request = new UpdateExpenseRequestModel();
        var command = new UpdateExpenseCommand(id, ExpenseModel());
        _converter.Setup(converter => converter.ToUpdateCommand(id, request)).Returns(command);
        _sender
            .Setup(sender => sender.Send(command, It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Put(id, request, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _sender.Verify(sender => sender.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_SendsCommandWithId_AndReturnsOk()
    {
        var id = Guid.NewGuid();
        _sender
            .Setup(sender => sender.Send(It.Is<DeleteExpenseCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }

    private static ExpenseCommandModel ExpenseModel() =>
        new ExpenseCommandModel(
            new DateTime(2024, 2, 3),
            ExpenseCategory.Other,
            "Misc",
            1m,
            PaymentMethod.Cash,
            "Acme",
            null);
}
