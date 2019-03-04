using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SuppliesListResponseResolver : IValueResolver<ValidationResponse<IEnumerable<Supply>>, SuppliesListResponseModel, IEnumerable<SupplyListModel>>
    {
        public IEnumerable<SupplyListModel> Resolve(
            ValidationResponse<IEnumerable<Supply>> source,
            SuppliesListResponseModel destination,
            IEnumerable<SupplyListModel> destMember,
            ResolutionContext context)
        {
            return source.Result?.Select(supply => new SupplyListModel
            {
                Id = supply.Id,
                Description = supply.Description,
                SupplierName = supply.Supplier?.Name,
                ReceivedDate = supply.ReceivedDate,
                Cost = supply.Cost,
                TotalCost = supply.TotalCost,
                ProductsAmount = supply.Products.Sum(prod => prod.Quantity),
            });
        }
    }
}