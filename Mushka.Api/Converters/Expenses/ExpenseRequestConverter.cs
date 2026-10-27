using System;
using Mushka.Api.ClientModels.Expense;
using Mushka.Application.Expenses.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Api.Converters.Expenses;

public sealed class ExpenseRequestConverter : IExpenseRequestConverter
{
    public CreateExpenseCommand ToCreateCommand(CreateExpenseRequestModel request) =>
        new CreateExpenseCommand(ToExpense(
            request.CreatedOn,
            request.Category,
            request.Purpose,
            request.Cost,
            request.CostMethod,
            request.SupplierName,
            request.Notes));

    public UpdateExpenseCommand ToUpdateCommand(Guid id, UpdateExpenseRequestModel request) =>
        new UpdateExpenseCommand(
            id,
            ToExpense(
                request.CreatedOn,
                request.Category,
                request.Purpose,
                request.Cost,
                request.CostMethod,
                request.SupplierName,
                request.Notes));

    private static ExpenseData ToExpense(
        DateTime createdOn,
        ExpenseCategory category,
        string purpose,
        decimal cost,
        PaymentMethod costMethod,
        string supplierName,
        string notes) =>
        new ExpenseData(createdOn, category, purpose, cost, costMethod, supplierName, notes);
}
