using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Supplies.Commands;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Converters;

public class UpdateSupplyConverterTests
{
    private readonly UpdateSupplyConverter _converter = new UpdateSupplyConverter();

    [Fact]
    public void ToSupply_UsesCommandId_AndMapsProducts()
    {
        var supplyId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateSupplyCommand(
            supplyId,
            new SupplyData(Guid.NewGuid(), new DateTime(2024, 4, 1), new DateTime(2024, 4, 2), "desc", null, null, 8, PaymentMethod.Cash, null, null, null, 8, "n"),
            new List<SupplyProductData> { new SupplyProductData(productId, 2, 3, 4) });

        var supply = _converter.ToSupply(command);

        supply.Id.Should().Be(supplyId);
        supply.Description.Should().Be("desc");
        supply.Cost.Should().Be(8);
        supply.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new SupplyProduct
            {
                SupplyId = supplyId,
                ProductId = productId,
                Quantity = 2,
                UnitPrice = 3,
                CostPrice = 4
            });
    }
}
