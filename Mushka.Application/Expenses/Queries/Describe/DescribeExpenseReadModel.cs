using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Queries.Describe;

public sealed class DescribeExpenseReadModel
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public ExpenseCategory Category { get; set; }

    public string Purpose { get; set; }

    public decimal Cost { get; set; }

    public PaymentMethod CostMethod { get; set; }

    public string Notes { get; set; }

    public string SupplierName { get; set; }
}
