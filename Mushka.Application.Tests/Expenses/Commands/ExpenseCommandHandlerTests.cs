using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Expenses.Commands;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Commands;

public class ExpenseCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IExpenseRepository> _repository = new Mock<IExpenseRepository>();
    private readonly Mock<ICreateExpenseConverter> _createConverter = new Mock<ICreateExpenseConverter>();
    private readonly Mock<IUpdateExpenseConverter> _updateConverter = new Mock<IUpdateExpenseConverter>();
    private readonly ExpenseCommandHandler _handler;

    public ExpenseCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IExpenseRepository>()).Returns(_repository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new ExpenseCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_AddsExpenseAndSaves()
    {
        var command = CreateCommand();
        var expense = new Expense { Purpose = "Ads" };
        _createConverter.Setup(converter => converter.ToExpense(command)).Returns(expense);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Add(expense), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenExpenseIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid());
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExpenseNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_ReplacesExpenseFromRequestAndSaves()
    {
        var command = UpdateCommand(Guid.NewGuid());
        var existing = new Expense { Id = command.Id, CreatedOn = new DateTime(2020, 1, 1), Purpose = "Old" };
        var updated = new Expense { Id = command.Id, CreatedOn = new DateTime(2024, 2, 3), Purpose = "New" };
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _updateConverter.Setup(converter => converter.ToExpense(command)).Returns(updated);

        await _handler.Handle(command, CancellationToken.None);

        updated.CreatedOn.Should().Be(new DateTime(2024, 2, 3));
        _repository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenExpenseIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteExpenseCommand(Guid.NewGuid());
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExpenseNotFound);
        _repository.Verify(repository => repository.Delete(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeletesExpenseAndSaves()
    {
        var expense = new Expense { Id = Guid.NewGuid() };
        var command = new DeleteExpenseCommand(expense.Id);
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expense);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Delete(expense), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static CreateExpenseCommand CreateCommand() =>
        new CreateExpenseCommand(ExpenseModel());

    private static UpdateExpenseCommand UpdateCommand(Guid id) =>
        new UpdateExpenseCommand(id, ExpenseModel());

    private static ExpenseData ExpenseModel() =>
        new ExpenseData(
            new DateTime(2024, 2, 3),
            ExpenseCategory.Advertising,
            "Ads",
            10m,
            PaymentMethod.Cash,
            "Acme",
            null);
}
