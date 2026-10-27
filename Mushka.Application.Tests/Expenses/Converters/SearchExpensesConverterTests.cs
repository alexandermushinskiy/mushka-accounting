using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Expenses.Converters;

public class SearchExpensesConverterTests
{
    private readonly SearchExpensesConverter _converter = new SearchExpensesConverter();

    [Fact]
    public void ToReadModel_MapsSearchFieldsAndTotal()
    {
        var expenseId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 3, 4);
        var expenses = new List<Expense>
        {
            new Expense
            {
                Id = expenseId,
                CreatedOn = createdOn,
                Category = ExpenseCategory.Promo,
                Purpose = "Flyers",
                Cost = 40m,
                CostMethod = PaymentMethod.Cash,
                SupplierName = "Print",
                Notes = "hidden from search"
            }
        };

        var readModel = _converter.ToReadModel(expenses);

        readModel.Total.Should().Be(1);
        readModel.Items.Should().ContainSingle().Which.Should().BeEquivalentTo(new
        {
            Id = expenseId,
            CreatedOn = createdOn,
            Category = ExpenseCategory.Promo,
            Purpose = "Flyers",
            Cost = 40m,
            SupplierName = "Print"
        });
    }
}
