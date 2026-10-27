using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Converters;

public class DescribeSupplyConverterTests
{
    private readonly DescribeSupplyConverter _converter = new DescribeSupplyConverter();

    [Fact]
    public void ToReadModel_MapsSupplyAndProducts()
    {
        var supplyId = Guid.NewGuid();
        var supplierId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var supply = new Supply
        {
            Id = supplyId,
            SupplierId = supplierId,
            Supplier = new Supplier { Name = "Acme" },
            Description = "winter",
            RequestDate = new DateTime(2024, 1, 2),
            ReceivedDate = new DateTime(2024, 1, 3),
            Cost = 10,
            CostMethod = PaymentMethod.Cash,
            Prepayment = 2,
            PrepaymentMethod = PaymentMethod.LiqPay,
            BankFee = 1,
            DeliveryCost = 3,
            DeliveryCostMethod = PaymentMethod.TransferToCard,
            TotalCost = 14,
            Notes = "note",
            Products = new List<SupplyProduct>
            {
                new SupplyProduct
                {
                    ProductId = productId,
                    Quantity = 2,
                    UnitPrice = 4,
                    CostPrice = 5,
                    Product = new Product
                    {
                        Name = "Sock",
                        VendorCode = "S-1",
                        Size = new Size { Name = "M" }
                    }
                },
                new SupplyProduct { ProductId = Guid.NewGuid(), Quantity = 3 }
            }
        };

        var readModel = _converter.ToReadModel(supply);

        readModel.Supply.Id.Should().Be(supplyId);
        readModel.Supply.SupplierId.Should().Be(supplierId);
        readModel.Supply.SupplierName.Should().Be("Acme");
        readModel.Supply.Description.Should().Be("winter");
        readModel.Supply.ProductsAmount.Should().Be(5);
        readModel.Supply.TotalCost.Should().Be(14);
        readModel.Supply.Notes.Should().Be("note");
        readModel.Products.Should().HaveCount(2);
        var product = readModel.Products.Single(item => item.Product.Id == productId);
        product.Quantity.Should().Be(2);
        product.UnitPrice.Should().Be(4);
        product.CostPrice.Should().Be(5);
        product.Product.Name.Should().Be("Sock");
        product.Product.VendorCode.Should().Be("S-1");
        product.Product.SizeName.Should().Be("M");
    }
}
