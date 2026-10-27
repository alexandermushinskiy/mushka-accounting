using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.Supplies.Commands;
using Mushka.Application.Supplies.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Converters;

public class CreateSupplyConverterTests
{
    private readonly Guid _supplyId = Guid.Parse("00000000-0000-0000-0000-000000000010");
    private readonly CreateSupplyConverter _converter;

    public CreateSupplyConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_supplyId);
        _converter = new CreateSupplyConverter(guidProvider.Object);
    }

    [Fact]
    public void ToSupply_MapsCommand_AndAssignsGeneratedId()
    {
        var supplierId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var requestDate = new DateTime(2024, 1, 2);
        var receivedDate = new DateTime(2024, 1, 5);
        var command = new CreateSupplyCommand(
            new SupplyData(
                supplierId,
                requestDate,
                receivedDate,
                "winter",
                10,
                PaymentMethod.Cash,
                20,
                PaymentMethod.LiqPay,
                3,
                PaymentMethod.TransferToCard,
                1,
                24,
                "note"),
            new List<SupplyProductData>
            {
                new SupplyProductData(productId, 4, 5, 6)
            });

        var supply = _converter.ToSupply(command);

        supply.Id.Should().Be(_supplyId);
        supply.SupplierId.Should().Be(supplierId);
        supply.RequestDate.Should().Be(requestDate);
        supply.ReceivedDate.Should().Be(receivedDate);
        supply.Description.Should().Be("winter");
        supply.Prepayment.Should().Be(10);
        supply.PrepaymentMethod.Should().Be(PaymentMethod.Cash);
        supply.Cost.Should().Be(20);
        supply.CostMethod.Should().Be(PaymentMethod.LiqPay);
        supply.DeliveryCost.Should().Be(3);
        supply.DeliveryCostMethod.Should().Be(PaymentMethod.TransferToCard);
        supply.BankFee.Should().Be(1);
        supply.TotalCost.Should().Be(24);
        supply.Notes.Should().Be("note");
        supply.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new SupplyProduct
            {
                SupplyId = _supplyId,
                ProductId = productId,
                Quantity = 4,
                UnitPrice = 5,
                CostPrice = 6
            });
    }

    [Fact]
    public void ToSupply_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var command = new CreateSupplyCommand(
            new SupplyData(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, "desc", null, null, 0, PaymentMethod.Cash, null, null, null, 0, null),
            null);

        var supply = _converter.ToSupply(command);

        supply.Products.Should().BeEmpty();
    }
}
