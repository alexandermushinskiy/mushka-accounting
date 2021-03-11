using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses.Describe;

namespace Mushka.WebApi.Resolvers.Expenses
{
    public class DescribeExpenseResponseConverter :
        ITypeConverter<OperationResult<Expense>, DescribeExpenseResponseModel>
    {
        public DescribeExpenseResponseModel Convert(
            OperationResult<Expense> source,
            DescribeExpenseResponseModel destination,
            ResolutionContext context)
        {
            return new DescribeExpenseResponseModel
            {
                Id = source.Data.Id,
                CreatedOn = source.Data.CreatedOn,
                Cost = source.Data.Cost,
                CostMethod = source.Data.CostMethod,
                Category = source.Data.Category,
                Purpose = source.Data.Purpose,
                SupplierName = source.Data.SupplierName,
                Notes = source.Data.Notes
            };
        }
    }
}