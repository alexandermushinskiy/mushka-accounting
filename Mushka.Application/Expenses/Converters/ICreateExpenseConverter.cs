using Mushka.Application.Expenses.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public interface ICreateExpenseConverter
{
    Expense ToExpense(CreateExpenseCommand command);
}
