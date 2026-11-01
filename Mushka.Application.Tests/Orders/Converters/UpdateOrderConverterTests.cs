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

public class UpdateOrderConverterTests
{
    private readonly Guid _generatedCustomerId = Guid.Parse("00000000-0000-0000-0000-000000000051");
    private readonly UpdateOrderConverter _converter;

    public UpdateOrderConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_generatedCustomerId);
        _converter = new UpdateOrderConverter(guidProvider.Object);
    }

    [Fact]
    public void ToOrder_UsesCommandId_AndAssignsMissingCustomerId()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateOrderCommand(
            orderId,
            new OrderData("O-2", new DateTime(2024, 4, 2), 90, PaymentMethod.LiqPay, 0, 10, "Lvivska", "Lviv", false, null),
            new OrderCustomerData(null, "Bob", "Stone", "456", null),
            new List<OrderProductData> { new OrderProductData(productId, 1, 90, 40) });

        var order = _converter.ToOrder(command);

        order.Id.Should().Be(orderId);
        order.Number.Should().Be("O-2");
        order.Discount.Should().BeNull();
        order.Customer.Id.Should().Be(_generatedCustomerId);
        order.Products.Should().ContainSingle()
            .Which.OrderId.Should().Be(orderId);
        order.Products.Should().ContainSingle()
            .Which.ProductId.Should().Be(productId);
    }
}
