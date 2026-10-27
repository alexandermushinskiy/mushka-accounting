using Mediator;

namespace Mushka.Application.Expenses.Commands;

public sealed record CreateExpenseCommand(ExpenseData Expense) : ICommand;
