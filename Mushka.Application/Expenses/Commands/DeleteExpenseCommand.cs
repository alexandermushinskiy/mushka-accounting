using System;
using Mediator;

namespace Mushka.Application.Expenses.Commands;

public sealed record DeleteExpenseCommand(Guid Id) : ICommand;
