using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Api.ClientModels.CorporateOrder;
using Mushka.Application.CorporateOrders.Commands;

namespace Mushka.Api.Converters.CorporateOrders;

public sealed class CorporateOrderRequestConverter : ICorporateOrderRequestConverter
{
    public CreateCorporateOrderCommand ToCreateCommand(CorporateOrderRequestModel request) =>
        new CreateCorporateOrderCommand(ToOrder(request), ToProducts(request.Products));

    public UpdateCorporateOrderCommand ToUpdateCommand(Guid id, CorporateOrderRequestModel request) =>
        new UpdateCorporateOrderCommand(id, ToOrder(request), ToProducts(request.Products));

    private static CorporateOrderData ToOrder(CorporateOrderRequestModel request) =>
        new CorporateOrderData(
            request.CreatedOn,
            request.OrderNumber,
            request.Cost,
            request.CostMethod,
            request.Prepayment,
            request.PrepaymentMethod,
            request.DeliveryCost,
            request.DeliveryCostMethod,
            request.Tax,
            request.Profit,
            request.Region,
            request.City,
            request.CompanyName,
            request.ContactPerson,
            request.Phone,
            request.Email,
            request.Notes);

    private static IReadOnlyList<CorporateOrderProductData> ToProducts(IEnumerable<CorporateOrderProductRequestModel> products) =>
        (products ?? Enumerable.Empty<CorporateOrderProductRequestModel>())
            .Select(product => new CorporateOrderProductData(
                product.Name,
                product.Quantity,
                product.UnitPrice,
                product.CostPrice))
            .ToList();
}
