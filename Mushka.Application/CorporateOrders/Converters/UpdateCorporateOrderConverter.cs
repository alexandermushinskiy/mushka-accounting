using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.CorporateOrders.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public sealed class UpdateCorporateOrderConverter : IUpdateCorporateOrderConverter
{
    private readonly IGuidProvider _guidProvider;

    public UpdateCorporateOrderConverter(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public CorporateOrder ToCorporateOrder(UpdateCorporateOrderCommand command)
    {
        return new CorporateOrder
        {
            Id = command.Id,
            CreatedOn = command.Order.CreatedOn,
            Number = command.Order.OrderNumber,
            Cost = command.Order.Cost,
            CostMethod = command.Order.CostMethod,
            Prepayment = command.Order.Prepayment,
            PrepaymentMethod = command.Order.PrepaymentMethod,
            DeliveryCost = command.Order.DeliveryCost,
            DeliveryCostMethod = command.Order.DeliveryCostMethod,
            Tax = command.Order.Tax,
            Profit = command.Order.Profit,
            Region = command.Order.Region,
            City = command.Order.City,
            CompanyName = command.Order.CompanyName,
            ContactPerson = command.Order.ContactPerson,
            Email = command.Order.Email,
            Phone = command.Order.Phone,
            Notes = command.Order.Notes,
            Products = ToProducts(command.Id, command.Products)
        };
    }

    private List<CorporateOrderProduct> ToProducts(Guid corporateOrderId, IEnumerable<CorporateOrderProductData> products) =>
        (products ?? Enumerable.Empty<CorporateOrderProductData>())
            .Select(product => new CorporateOrderProduct
            {
                Id = _guidProvider.NewGuid(),
                CorporateOrderId = corporateOrderId,
                Name = product.Name,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice
            })
            .ToList();
}
