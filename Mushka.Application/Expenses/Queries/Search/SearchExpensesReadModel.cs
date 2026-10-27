using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Queries.Search;

public sealed class SearchExpensesReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchExpenseReadModel> Items { get; set; }
}

public sealed class SearchExpenseReadModel
{
    public Guid Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public ExpenseCategory Category { get; set; }

    public string Purpose { get; set; }

    public decimal Cost { get; set; }

    public string SupplierName { get; set; }
}
