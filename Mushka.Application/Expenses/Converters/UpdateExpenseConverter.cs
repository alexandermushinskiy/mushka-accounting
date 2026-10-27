using Mushka.Application.Expenses.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public sealed class UpdateExpenseConverter : IUpdateExpenseConverter
{
    public Expense ToExpense(UpdateExpenseCommand command) =>
        new Expense
        {
            Id = command.Id,
            CreatedOn = command.Expense.CreatedOn,
            Category = command.Expense.Category,
            Purpose = command.Expense.Purpose,
            Cost = command.Expense.Cost,
            CostMethod = command.Expense.CostMethod,
            SupplierName = command.Expense.SupplierName,
            Notes = command.Expense.Notes
        };
}
