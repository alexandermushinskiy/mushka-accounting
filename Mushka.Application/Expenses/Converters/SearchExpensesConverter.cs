using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Expenses.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public sealed class SearchExpensesConverter : ISearchExpensesConverter
{
    public SearchExpensesReadModel ToReadModel(IEnumerable<Expense> expenses)
    {
        var items = expenses.Select(ToItem).ToList();

        return new SearchExpensesReadModel
        {
            Total = items.Count,
            Items = items
        };
    }

    private static SearchExpenseReadModel ToItem(Expense expense) =>
        new SearchExpenseReadModel
        {
            Id = expense.Id,
            CreatedOn = expense.CreatedOn,
            Category = expense.Category,
            Purpose = expense.Purpose,
            Cost = expense.Cost,
            SupplierName = expense.SupplierName
        };
}
