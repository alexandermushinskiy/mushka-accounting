using System;
using Mediator;

namespace Mushka.Application.Expenses.Commands;

public sealed record UpdateExpenseCommand(Guid Id, ExpenseData Expense) : ICommand;
