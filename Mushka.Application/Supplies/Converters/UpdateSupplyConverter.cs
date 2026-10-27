using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Supplies.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters;

public sealed class UpdateSupplyConverter : IUpdateSupplyConverter
{
    public Supply ToSupply(UpdateSupplyCommand command)
    {
        return new Supply
        {
            Id = command.Id,
            SupplierId = command.Supply.SupplierId,
            RequestDate = command.Supply.RequestDate,
            ReceivedDate = command.Supply.ReceivedDate,
            Description = command.Supply.Description,
            Cost = command.Supply.Cost,
            CostMethod = command.Supply.CostMethod,
            DeliveryCost = command.Supply.DeliveryCost,
            DeliveryCostMethod = command.Supply.DeliveryCostMethod,
            Prepayment = command.Supply.Prepayment,
            PrepaymentMethod = command.Supply.PrepaymentMethod,
            BankFee = command.Supply.BankFee,
            TotalCost = command.Supply.TotalCost,
            Notes = command.Supply.Notes,
            Products = ToProducts(command.Id, command.Products)
        };
    }

    private static List<SupplyProduct> ToProducts(Guid supplyId, IEnumerable<SupplyProductData> products) =>
        (products ?? Enumerable.Empty<SupplyProductData>())
            .Select(product => new SupplyProduct
            {
                SupplyId = supplyId,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice
            })
            .ToList();
}
