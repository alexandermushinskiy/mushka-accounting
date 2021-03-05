using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Dto;
using Mushka.WebApi.ClientModels.Supply.Search;

namespace Mushka.WebApi.Resolvers.Supplies
{
    public class SearchSuppliesResponseConverter : ITypeConverter<OperationResult<ItemsWithTotalCount<Supply>>, SearchSuppliesResponseModel>
    {
        public SearchSuppliesResponseModel Convert(
            OperationResult<ItemsWithTotalCount<Supply>> source,
            SearchSuppliesResponseModel destination,
            ResolutionContext context)
        {
            return new SearchSuppliesResponseModel
            {
                Total = source.Data.TotalCount,
                Items = source.Data.Data.Select(Convert)
            };
        }

        private static SearchSupplyResponseModel Convert(Supply supply) =>
            new SearchSupplyResponseModel
            {
                Id = supply.Id,
                Description = supply.Description,
                SupplierName = supply.Supplier?.Name,
                ReceivedDate = supply.ReceivedDate,
                Cost = supply.Cost,
                TotalCost = supply.TotalCost,
                ProductsAmount = supply.Products.Sum(prod => prod.Quantity),
            };
    }
}