using System;
using FluentAssertions;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Converters;

public class DescribeExpenseConverterTests
{
    private readonly DescribeExpenseConverter _converter = new DescribeExpenseConverter();

    [Fact]
    public void ToReadModel_MapsExpense()
    {
        var expenseId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 3, 4);
        var expense = new Expense
        {
            Id = expenseId,
            CreatedOn = createdOn,
            Category = ExpenseCategory.WebSite,
            Purpose = "Hosting",
            Cost = 15m,
            CostMethod = PaymentMethod.TransferToBankAccount,
            SupplierName = "Host",
            Notes = "yearly"
        };

        var readModel = _converter.ToReadModel(expense);

        readModel.Id.Should().Be(expenseId);
        readModel.CreatedOn.Should().Be(createdOn);
        readModel.Category.Should().Be(ExpenseCategory.WebSite);
        readModel.Purpose.Should().Be("Hosting");
        readModel.Cost.Should().Be(15m);
        readModel.CostMethod.Should().Be(PaymentMethod.TransferToBankAccount);
        readModel.SupplierName.Should().Be("Host");
        readModel.Notes.Should().Be("yearly");
    }
}
