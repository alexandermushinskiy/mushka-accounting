using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Expense;
using Mushka.Api.Converters.Expenses;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Api.Tests.Converters.Expenses;

public class ExpenseRequestConverterTests
{
    private readonly ExpenseRequestConverter _converter = new ExpenseRequestConverter();
    private readonly DateTime _createdOn = new DateTime(2024, 5, 6);

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var request = new CreateExpenseRequestModel
        {
            CreatedOn = _createdOn,
            Category = ExpenseCategory.Photography,
            Purpose = "Shoot",
            Cost = 200m,
            CostMethod = PaymentMethod.Cash,
            SupplierName = "Studio",
            Notes = "outdoor"
        };

        var command = _converter.ToCreateCommand(request);

        command.Expense.Should().BeEquivalentTo(new
        {
            CreatedOn = _createdOn,
            Category = ExpenseCategory.Photography,
            Purpose = "Shoot",
            Cost = 200m,
            CostMethod = PaymentMethod.Cash,
            SupplierName = "Studio",
            Notes = "outdoor"
        });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = new UpdateExpenseRequestModel
        {
            CreatedOn = _createdOn,
            Category = ExpenseCategory.Other,
            Purpose = "Misc",
            Cost = 5m,
            CostMethod = PaymentMethod.LiqPay,
            SupplierName = "Shop",
            Notes = null
        };

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Expense.Purpose.Should().Be("Misc");
        command.Expense.CreatedOn.Should().Be(_createdOn);
        command.Expense.CostMethod.Should().Be(PaymentMethod.LiqPay);
        command.Expense.Notes.Should().BeNull();
    }
}
