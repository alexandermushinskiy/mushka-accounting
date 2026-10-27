using System;
using Mushka.Api.ClientModels.Expense;
using Mushka.Application.Expenses.Commands;

namespace Mushka.Api.Converters.Expenses;

public interface IExpenseRequestConverter
{
    CreateExpenseCommand ToCreateCommand(CreateExpenseRequestModel request);

    UpdateExpenseCommand ToUpdateCommand(Guid id, UpdateExpenseRequestModel request);
}
