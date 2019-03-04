using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers
{
    public class ExpenseRequestConverter : ITypeConverter<ExpenseRequestModel, Expense>
    {
        private readonly IGuidProvider guidProvider;

        public ExpenseRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Expense Convert(ExpenseRequestModel source, Expense destination, ResolutionContext context) =>
            new Expense
            {
                Id = context.GetId() ?? guidProvider.NewGuid(),
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