using System.Linq;
using Mushka.Application.Supplies.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters;

public sealed class DescribeSupplyConverter : IDescribeSupplyConverter
{
    public DescribeSupplyReadModel ToReadModel(Supply supply) =>
        new DescribeSupplyReadModel
        {
            Supply = ToSupply(supply),
            Products = supply.Products?.Select(ToSupplyProduct) ?? Enumerable.Empty<DescribeSupplyProductReadModel>()
        };

    private static DescribeSupplyDetailsReadModel ToSupply(Supply supply) =>
        new DescribeSupplyDetailsReadModel
        {
            Id = supply.Id,
            SupplierId = supply.SupplierId,
            SupplierName = supply.Supplier?.Name,
            Description = supply.Description,
            RequestDate = supply.RequestDate,
            ReceivedDate = supply.ReceivedDate,
            Cost = supply.Cost,
            CostMethod = supply.CostMethod,
            Prepayment = supply.Prepayment,
            PrepaymentMethod = supply.PrepaymentMethod,
            BankFee = supply.BankFee,
            DeliveryCost = supply.DeliveryCost,
            DeliveryCostMethod = supply.DeliveryCostMethod,
            TotalCost = supply.TotalCost,
            ProductsAmount = supply.Products?.Sum(product => product.Quantity) ?? 0,
            Notes = supply.Notes
        };

    private static DescribeSupplyProductReadModel ToSupplyProduct(SupplyProduct supplyProduct) =>
        new DescribeSupplyProductReadModel
        {
            UnitPrice = supplyProduct.UnitPrice,
            Quantity = supplyProduct.Quantity,
            CostPrice = supplyProduct.CostPrice,
            Product = new DescribeSupplyProductDetailsReadModel
            {
                Id = supplyProduct.ProductId,
                Name = supplyProduct.Product?.Name,
                VendorCode = supplyProduct.Product?.VendorCode,
                SizeName = supplyProduct.Product?.Size?.Name
            }
        };
}
