using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Api.ClientModels.Supply;
using Mushka.Application.Supplies.Commands;

namespace Mushka.Api.Converters.Supplies;

public sealed class SupplyRequestConverter : ISupplyRequestConverter
{
    public CreateSupplyCommand ToCreateCommand(SupplyRequestModel request) =>
        new CreateSupplyCommand(ToSupply(request), ToProducts(request.Products));

    public UpdateSupplyCommand ToUpdateCommand(Guid id, SupplyRequestModel request) =>
        new UpdateSupplyCommand(id, ToSupply(request), ToProducts(request.Products));

    private static SupplyCommandModel ToSupply(SupplyRequestModel request) =>
        new SupplyCommandModel(
            request.SupplierId,
            request.RequestDate,
            request.ReceivedDate,
            request.Description,
            request.Prepayment,
            request.PrepaymentMethod,
            request.Cost,
            request.CostMethod,
            request.DeliveryCost,
            request.DeliveryCostMethod,
            request.BankFee,
            request.TotalCost,
            request.Notes);

    private static IReadOnlyList<SupplyProductCommandModel> ToProducts(IEnumerable<SupplyProductRequestModel> products) =>
        (products ?? Enumerable.Empty<SupplyProductRequestModel>())
            .Select(product => new SupplyProductCommandModel(
                product.ProductId,
                product.Quantity,
                product.UnitPrice,
                product.CostPrice))
            .ToList();
}
