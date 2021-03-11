using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses.Search;

namespace Mushka.WebApi.Resolvers.Expenses
{
    public class SearchExpensesResponseConverter :
        ITypeConverter<OperationResult<IEnumerable<Expense>>, SearchExpensesResponseModel>
    {
        public SearchExpensesResponseModel Convert(
            OperationResult<IEnumerable<Expense>> source,
            SearchExpensesResponseModel destination,
            ResolutionContext context)
        {
            return new SearchExpensesResponseModel
            {
                Total = source.Data.Count(),
                Items = source.Data.Select(ConvertToSearchExpenseModel)
            };
        }

        private SearchExpenseModel ConvertToSearchExpenseModel(Expense expense)
        {
            return new SearchExpenseModel
            {
                Id = expense.Id,
                CreatedOn = expense.CreatedOn,
                Cost = expense.Cost,
                Category = expense.Category,
                Purpose = expense.Purpose,
                SupplierName = expense.SupplierName
            };
        }
    }
}