using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.CorporateOrders.Commands;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Converters;

public class UpdateCorporateOrderConverterTests
{
    private readonly Guid _productId = Guid.Parse("00000000-0000-0000-0000-000000000019");
    private readonly UpdateCorporateOrderConverter _converter;

    public UpdateCorporateOrderConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_productId);
        _converter = new UpdateCorporateOrderConverter(guidProvider.Object);
    }

    [Fact]
    public void ToCorporateOrder_UsesCommandId_AndAssignsProductIds()
    {
        var orderId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 4, 2);
        var command = new UpdateCorporateOrderCommand(
            orderId,
            new CorporateOrderData(
                createdOn,
                "CO-2",
                300,
                PaymentMethod.TransferToBankAccount,
                null,
                null,
                10,
                PaymentMethod.Cash,
                null,
                90,
                "Lvivska",
                "Lviv",
                "Beta",
                "Bob",
                "456",
                null,
                "updated"),
            new List<CorporateOrderProductData>
            {
                new CorporateOrderProductData("Hat", 2, 70, 30)
            });

        var order = _converter.ToCorporateOrder(command);

        order.Id.Should().Be(orderId);
        order.CreatedOn.Should().Be(createdOn);
        order.Number.Should().Be("CO-2");
        order.Cost.Should().Be(300);
        order.CostMethod.Should().Be(PaymentMethod.TransferToBankAccount);
        order.DeliveryCost.Should().Be(10);
        order.CompanyName.Should().Be("Beta");
        order.Notes.Should().Be("updated");
        order.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new CorporateOrderProduct
            {
                Id = _productId,
                CorporateOrderId = orderId,
                Name = "Hat",
                Quantity = 2,
                UnitPrice = 70,
                CostPrice = 30
            });
    }

    [Fact]
    public void ToCorporateOrder_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var command = new UpdateCorporateOrderCommand(
            Guid.NewGuid(),
            new CorporateOrderData(
                DateTime.UtcNow,
                "CO-2",
                1,
                PaymentMethod.Cash,
                null,
                null,
                null,
                null,
                null,
                1,
                "Lvivska",
                "Lviv",
                "Beta",
                "Bob",
                "456",
                null,
                null),
            null);

        var order = _converter.ToCorporateOrder(command);

        order.Products.Should().BeEmpty();
    }
}
