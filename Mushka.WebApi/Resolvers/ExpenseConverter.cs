using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;

namespace Mushka.WebApi.Resolvers
{
    public class ExpenseConverter : ITypeConverter<Expense, ExpenseModel>
    {
        public ExpenseModel Convert(Expense source, ExpenseModel destination, ResolutionContext context) =>
            new ExpenseModel
            {
                Id = source.Id,
                CreatedOn = source.CreatedOn,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Category = source.Category,
                Purpose = source.Purpose,
                SupplierName = source.SupplierName,
                Notes = source.Notes
            };
    }
}