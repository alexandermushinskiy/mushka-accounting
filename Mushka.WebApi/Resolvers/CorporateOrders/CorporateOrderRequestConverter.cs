using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers.CorporateOrders
{
    public class CorporateOrderRequestConverter : ITypeConverter<CorporateOrderRequestModel, CorporateOrder>
    {
        private readonly IGuidProvider guidProvider;

        public CorporateOrderRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public CorporateOrder Convert(CorporateOrderRequestModel source, CorporateOrder destination, ResolutionContext context)
        {
            var corporateOrderId = context.GetId() ?? guidProvider.NewGuid();

            return new CorporateOrder
            {
                Id = corporateOrderId,
                CreatedOn = source.CreatedOn,
                Number = source.OrderNumber,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                DeliveryCost = source.DeliveryCost,
                DeliveryCostMethod = source.DeliveryCostMethod,
                Prepayment = source.Prepayment,
                PrepaymentMethod = source.PrepaymentMethod,
                Tax = source.Tax,
                Profit = source.Profit,
                Notes = source.Notes,
                Region = source.Region,
                City = source.City,
                CompanyName = source.CompanyName,
                ContactPerson = source.ContactPerson,
                Email = source.Email,
                Phone = source.Phone,
                Products = source.Products.Select(prod => new CorporateOrderProduct
                {
                    CorporateOrderId = corporateOrderId,
                    Name = prod.Name,
                    Quantity = prod.Quantity,
                    CostPrice = prod.CostPrice,
                    UnitPrice = prod.UnitPrice
                }).ToList()
            };
        }
    }
}