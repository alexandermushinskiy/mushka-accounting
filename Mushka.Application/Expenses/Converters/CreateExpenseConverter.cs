using Mushka.Application.Expenses.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public sealed class CreateExpenseConverter : ICreateExpenseConverter
{
    private readonly IGuidProvider _guidProvider;

    public CreateExpenseConverter(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Expense ToExpense(CreateExpenseCommand command) =>
        new Expense
        {
            Id = _guidProvider.NewGuid(),
            CreatedOn = command.Expense.CreatedOn,
            Category = command.Expense.Category,
            Purpose = command.Expense.Purpose,
            Cost = command.Expense.Cost,
            CostMethod = command.Expense.CostMethod,
            SupplierName = command.Expense.SupplierName,
            Notes = command.Expense.Notes
        };
}
