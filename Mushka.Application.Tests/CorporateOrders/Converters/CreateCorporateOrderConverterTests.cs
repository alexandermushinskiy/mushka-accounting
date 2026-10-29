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

public class CreateCorporateOrderConverterTests
{
    private readonly Guid _orderId = Guid.Parse("00000000-0000-0000-0000-000000000011");
    private readonly Guid _productId = Guid.Parse("00000000-0000-0000-0000-000000000012");
    private readonly CreateCorporateOrderConverter _converter;

    public CreateCorporateOrderConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.SetupSequence(provider => provider.NewGuid())
            .Returns(_orderId)
            .Returns(_productId);
        _converter = new CreateCorporateOrderConverter(guidProvider.Object);
    }

    [Fact]
    public void ToCorporateOrder_MapsCommand_AndAssignsGeneratedIds()
    {
        var createdOn = new DateTime(2024, 3, 1);
        var command = new CreateCorporateOrderCommand(
            new CorporateOrderData(
                createdOn,
                "CO-1",
                250,
                PaymentMethod.TransferToCard,
                50,
                PaymentMethod.Cash,
                20,
                PaymentMethod.LiqPay,
                5,
                80,
                "Kyivska",
                "Kyiv",
                "Acme",
                "Ann",
                "123",
                "ann@example.com",
                "note"),
            new List<CorporateOrderProductData>
            {
                new CorporateOrderProductData("Socks", 3, 40, 15)
            });

        var order = _converter.ToCorporateOrder(command);

        order.Id.Should().Be(_orderId);
        order.CreatedOn.Should().Be(createdOn);
        order.Number.Should().Be("CO-1");
        order.Cost.Should().Be(250);
        order.CostMethod.Should().Be(PaymentMethod.TransferToCard);
        order.Prepayment.Should().Be(50);
        order.PrepaymentMethod.Should().Be(PaymentMethod.Cash);
        order.DeliveryCost.Should().Be(20);
        order.DeliveryCostMethod.Should().Be(PaymentMethod.LiqPay);
        order.Tax.Should().Be(5);
        order.Profit.Should().Be(80);
        order.Region.Should().Be("Kyivska");
        order.City.Should().Be("Kyiv");
        order.CompanyName.Should().Be("Acme");
        order.ContactPerson.Should().Be("Ann");
        order.Phone.Should().Be("123");
        order.Email.Should().Be("ann@example.com");
        order.Notes.Should().Be("note");
        order.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new CorporateOrderProduct
            {
                Id = _productId,
                CorporateOrderId = _orderId,
                Name = "Socks",
                Quantity = 3,
                UnitPrice = 40,
                CostPrice = 15
            });
    }

    [Fact]
    public void ToCorporateOrder_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var command = new CreateCorporateOrderCommand(
            new CorporateOrderData(
                DateTime.UtcNow,
                "CO-1",
                1,
                PaymentMethod.Cash,
                null,
                null,
                null,
                null,
                null,
                1,
                "Kyivska",
                "Kyiv",
                "Acme",
                "Ann",
                "123",
                null,
                null),
            null);

        var order = _converter.ToCorporateOrder(command);

        order.Products.Should().BeEmpty();
    }
}
