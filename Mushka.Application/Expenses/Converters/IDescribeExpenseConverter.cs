using Mushka.Application.Expenses.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public interface IDescribeExpenseConverter
{
    DescribeExpenseReadModel ToReadModel(Expense expense);
}
