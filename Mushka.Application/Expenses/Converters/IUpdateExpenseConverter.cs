using Mushka.Application.Expenses.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public interface IUpdateExpenseConverter
{
    Expense ToExpense(UpdateExpenseCommand command);
}
