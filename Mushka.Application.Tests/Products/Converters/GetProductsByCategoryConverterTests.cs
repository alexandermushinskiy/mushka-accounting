using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class GetProductsByCategoryConverterTests
{
    private readonly GetProductsByCategoryConverter _converter = new GetProductsByCategoryConverter();

    [Fact]
    public void ToReadModel_MapsLastDeliveryFromTheLatestSupply()
    {
        var productId = Guid.NewGuid();
        var olderSupply = new Supply
        {
            ReceivedDate = new DateTime(2024, 1, 1),
            Products = new List<SupplyProduct> { new SupplyProduct { ProductId = productId, Quantity = 2 } }
        };
        var latestSupply = new Supply
        {
            ReceivedDate = new DateTime(2024, 3, 1),
            Products = new List<SupplyProduct> { new SupplyProduct { ProductId = productId, Quantity = 5 } }
        };
        var products = new List<Product>
        {
            new Product
            {
                Id = productId,
                Name = "Socks",
                VendorCode = "SK-1",
                RecommendedPrice = 10,
                CreatedOn = new DateTime(2023, 1, 1),
                Quantity = 8,
                Size = new Size { Name = "M" },
                Supplies = new List<SupplyProduct>
                {
                    new SupplyProduct { ProductId = productId, Quantity = 2, Supply = olderSupply },
                    new SupplyProduct { ProductId = productId, Quantity = 5, Supply = latestSupply }
                }
            }
        };

        var readModel = _converter.ToReadModel(products);

        var item = readModel.Items.Should().ContainSingle().Subject;
        item.Id.Should().Be(productId);
        item.Name.Should().Be("Socks");
        item.VendorCode.Should().Be("SK-1");
        item.Quantity.Should().Be(8);
        item.DeliveriesCount.Should().Be(2);
        item.LastDeliveryDate.Should().Be(new DateTime(2024, 3, 1));
        item.LastDeliveryCount.Should().Be(5);
        item.SizeName.Should().Be("M");
    }

    [Fact]
    public void ToReadModel_WhenProductHasNoSupplies_LeavesDeliveryFieldsEmpty()
    {
        var products = new List<Product>
        {
            new Product { Name = "Socks", Supplies = new List<SupplyProduct>() }
        };

        var item = _converter.ToReadModel(products).Items.Should().ContainSingle().Subject;

        item.DeliveriesCount.Should().Be(0);
        item.LastDeliveryDate.Should().BeNull();
        item.LastDeliveryCount.Should().BeNull();
        item.SizeName.Should().BeNull();
    }
}
