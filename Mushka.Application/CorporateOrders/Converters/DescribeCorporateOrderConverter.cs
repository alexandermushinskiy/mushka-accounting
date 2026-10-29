using System.Linq;
using Mushka.Application.CorporateOrders.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public sealed class DescribeCorporateOrderConverter : IDescribeCorporateOrderConverter
{
    public DescribeCorporateOrderReadModel ToReadModel(CorporateOrder corporateOrder) =>
        new DescribeCorporateOrderReadModel
        {
            Order = ToOrder(corporateOrder),
            Products = corporateOrder.Products?.Select(ToProduct) ?? Enumerable.Empty<DescribeCorporateOrderProductReadModel>()
        };

    private static DescribeCorporateOrderDetailsReadModel ToOrder(CorporateOrder corporateOrder) =>
        new DescribeCorporateOrderDetailsReadModel
        {
            Id = corporateOrder.Id,
            CreatedOn = corporateOrder.CreatedOn,
            OrderNumber = corporateOrder.Number,
            Cost = corporateOrder.Cost,
            CostMethod = corporateOrder.CostMethod,
            Prepayment = corporateOrder.Prepayment,
            PrepaymentMethod = corporateOrder.PrepaymentMethod,
            DeliveryCost = corporateOrder.DeliveryCost,
            DeliveryCostMethod = corporateOrder.DeliveryCostMethod,
            Tax = corporateOrder.Tax,
            Profit = corporateOrder.Profit,
            Notes = corporateOrder.Notes,
            CompanyName = corporateOrder.CompanyName,
            ContactPerson = corporateOrder.ContactPerson,
            Phone = corporateOrder.Phone,
            Email = corporateOrder.Email,
            Region = corporateOrder.Region,
            City = corporateOrder.City
        };

    private static DescribeCorporateOrderProductReadModel ToProduct(CorporateOrderProduct product) =>
        new DescribeCorporateOrderProductReadModel
        {
            Name = product.Name,
            Quantity = product.Quantity,
            CostPrice = product.CostPrice,
            UnitPrice = product.UnitPrice
        };
}
