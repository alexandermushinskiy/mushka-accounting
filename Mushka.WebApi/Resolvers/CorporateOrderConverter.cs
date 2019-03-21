using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;

namespace Mushka.WebApi.Resolvers
{
    public class CorporateOrderConverter : ITypeConverter<CorporateOrder, CorporateOrderModel>
    {
        public CorporateOrderModel Convert(CorporateOrder source, CorporateOrderModel destination, ResolutionContext context) =>
            new CorporateOrderModel
            {
                Id = source.Id,
                CreatedOn = source.CreatedOn,
                Number = source.Number,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Prepayment = source.Prepayment,
                PrepaymentMethod = source.PrepaymentMethod,
                DeliveryCost = source.DeliveryCost,
                DeliveryCostMethod = source.DeliveryCostMethod,
                Tax = source.Tax,
                Profit = source.Profit,
                Notes = source.Notes,
                CompanyName = source.CompanyName,
                ContactPerson = source.ContactPerson,
                Phone = source.Phone,
                Email = source.Email,
                Region = source.Region,
                City = source.City,
                Products = source.Products.Select(prod => new CorporateOrderProductModel
                {
                    Name = prod.Name,
                    Quantity = prod.Quantity,
                    CostPrice = prod.CostPrice,
                    UnitPrice = prod.UnitPrice
                })
            };
    }
}