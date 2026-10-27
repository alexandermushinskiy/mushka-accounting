using Mushka.Application.Expenses.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Expenses.Converters;

public sealed class DescribeExpenseConverter : IDescribeExpenseConverter
{
    public DescribeExpenseReadModel ToReadModel(Expense expense) =>
        new DescribeExpenseReadModel
        {
            Id = expense.Id,
            CreatedOn = expense.CreatedOn,
            Category = expense.Category,
            Purpose = expense.Purpose,
            Cost = expense.Cost,
            CostMethod = expense.CostMethod,
            SupplierName = expense.SupplierName,
            Notes = expense.Notes
        };
}
