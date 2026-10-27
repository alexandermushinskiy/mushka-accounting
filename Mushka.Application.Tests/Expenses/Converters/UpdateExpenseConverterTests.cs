using System;
using FluentAssertions;
using Mushka.Application.Expenses.Commands;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Converters;

public class UpdateExpenseConverterTests
{
    private readonly UpdateExpenseConverter _converter = new UpdateExpenseConverter();

    [Fact]
    public void ToExpense_UsesCommandId_AndMapsRequestFields()
    {
        var expenseId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 5, 6);
        var command = new UpdateExpenseCommand(
            expenseId,
            new ExpenseCommandModel(
                createdOn,
                ExpenseCategory.Equipment,
                "Camera",
                900m,
                PaymentMethod.TransferToCard,
                "Shop",
                "invoice"));

        var expense = _converter.ToExpense(command);

        expense.Should().BeEquivalentTo(new Expense
        {
            Id = expenseId,
            CreatedOn = createdOn,
            Category = ExpenseCategory.Equipment,
            Purpose = "Camera",
            Cost = 900m,
            CostMethod = PaymentMethod.TransferToCard,
            SupplierName = "Shop",
            Notes = "invoice"
        });
    }
}
