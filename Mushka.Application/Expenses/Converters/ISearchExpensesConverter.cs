using System.Collections.Generic;
using Mushka.Application.Expenses.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public interface ISearchExpensesConverter
{
    SearchExpensesReadModel ToReadModel(IEnumerable<Expense> expenses);
}
