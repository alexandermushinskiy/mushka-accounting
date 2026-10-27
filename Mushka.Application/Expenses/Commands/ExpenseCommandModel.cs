using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Commands;

public sealed record ExpenseCommandModel(
    DateTime CreatedOn,
    ExpenseCategory Category,
    string Purpose,
    decimal Cost,
    PaymentMethod CostMethod,
    string SupplierName,
    string Notes);
