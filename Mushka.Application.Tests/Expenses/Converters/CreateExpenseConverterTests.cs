using System;
using FluentAssertions;
using Moq;
using Mushka.Application.Expenses.Commands;
using Mushka.Application.Expenses.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Converters;

public class CreateExpenseConverterTests
{
    private readonly Guid _expenseId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private readonly CreateExpenseConverter _converter;

    public CreateExpenseConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_expenseId);
        _converter = new CreateExpenseConverter(guidProvider.Object);
    }

    [Fact]
    public void ToExpense_MapsCommand_AndAssignsGeneratedId()
    {
        var createdOn = new DateTime(2024, 5, 6);
        var command = new CreateExpenseCommand(new ExpenseCommandModel(
            createdOn,
            ExpenseCategory.Design,
            "Logo",
            125.5m,
            PaymentMethod.LiqPay,
            "Studio",
            "rush"));

        var expense = _converter.ToExpense(command);

        expense.Should().BeEquivalentTo(new Expense
        {
            Id = _expenseId,
            CreatedOn = createdOn,
            Category = ExpenseCategory.Design,
            Purpose = "Logo",
            Cost = 125.5m,
            CostMethod = PaymentMethod.LiqPay,
            SupplierName = "Studio",
            Notes = "rush"
        });
    }
}
