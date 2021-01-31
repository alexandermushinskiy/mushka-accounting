using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder.GetById;

namespace Mushka.WebApi.Resolvers.CorporateOrders
{
    public class CorporateOrderProductConverter : ITypeConverter<CorporateOrderProduct, CorporateOrderProductModel>
    {
        public CorporateOrderProductModel Convert(
            CorporateOrderProduct source,
            CorporateOrderProductModel destination,
            ResolutionContext context)
        {
            return new CorporateOrderProductModel
            {
                Name = source.Name,
                Quantity = source.Quantity,
                CostPrice = source.CostPrice,
                UnitPrice = source.UnitPrice
            };
        }
    }
}