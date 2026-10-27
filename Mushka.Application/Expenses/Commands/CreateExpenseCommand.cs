using Mediator;

namespace Mushka.Application.Expenses.Commands;

public sealed record CreateExpenseCommand(ExpenseCommandModel Expense) : ICommand;
