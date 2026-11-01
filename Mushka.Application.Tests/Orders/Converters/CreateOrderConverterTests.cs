using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Orders.Converters;

public class CreateOrderConverterTests
{
    private readonly Guid _orderId = Guid.Parse("00000000-0000-0000-0000-000000000041");
    private readonly Guid _customerId = Guid.Parse("00000000-0000-0000-0000-000000000042");
    private readonly CreateOrderConverter _converter;

    public CreateOrderConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.SetupSequence(provider => provider.NewGuid())
            .Returns(_orderId)
            .Returns(_customerId);
        _converter = new CreateOrderConverter(guidProvider.Object);
    }

    [Fact]
    public void ToOrder_MapsCommand_ClearsZeroDiscount_AndAssignsGeneratedIds()
    {
        var orderDate = new DateTime(2024, 3, 1);
        var productId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            new OrderData("O-1", orderDate, 250, PaymentMethod.Cash, 0, 80, "Kyivska", "Kyiv", true, "note"),
            new OrderCustomerData(null, "Ann", "Smith", "123", "ann@example.com"),
            new List<OrderProductData> { new OrderProductData(productId, 2, 40, 15) });

        var order = _converter.ToOrder(command);

        order.Id.Should().Be(_orderId);
        order.Number.Should().Be("O-1");
        order.OrderDate.Should().Be(orderDate);
        order.Cost.Should().Be(250);
        order.CostMethod.Should().Be(PaymentMethod.Cash);
        order.Discount.Should().BeNull();
        order.Profit.Should().Be(80);
        order.Region.Should().Be("Kyivska");
        order.City.Should().Be("Kyiv");
        order.IsWholesale.Should().BeTrue();
        order.Notes.Should().Be("note");
        order.Customer.Should().BeEquivalentTo(new Customer
        {
            Id = _customerId,
            FirstName = "Ann",
            LastName = "Smith",
            Phone = "123",
            Email = "ann@example.com"
        });
        order.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new OrderProduct
            {
                OrderId = _orderId,
                ProductId = productId,
                Quantity = 2,
                UnitPrice = 40,
                CostPrice = 15
            });
    }

    [Fact]
    public void ToOrder_WhenCustomerIdIsProvided_KeepsIt()
    {
        var customerId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            new OrderData("O-1", DateTime.UtcNow, 1, PaymentMethod.Cash, 10, 1, "Kyivska", "Kyiv", false, null),
            new OrderCustomerData(customerId, "Ann", "Smith", "123", null),
            null);

        var order = _converter.ToOrder(command);

        order.Discount.Should().Be(10);
        order.Customer.Id.Should().Be(customerId);
        order.Products.Should().BeEmpty();
    }
}
