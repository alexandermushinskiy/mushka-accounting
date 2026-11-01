using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Orders.Converters;
using Mushka.Domain.Dto;
using Xunit;

namespace Mushka.Application.Tests.Orders.Converters;

public class SearchOrdersConverterTests
{
    private readonly SearchOrdersConverter _converter = new SearchOrdersConverter();

    [Fact]
    public void ToReadModel_UsesProvidedTotal_AndMapsItems()
    {
        var orderId = Guid.NewGuid();
        var orderDate = new DateTime(2024, 3, 1);
        var orders = new List<OrderSummaryDto>
        {
            new OrderSummaryDto
            {
                Id = orderId,
                OrderDate = orderDate,
                OrderNumber = "O-1",
                Cost = 100,
                Address = "Kyivska, Kyiv",
                CustomerName = "Ann Smith",
                ProductsCount = 3,
                IsWholesale = true
            }
        };

        var readModel = _converter.ToReadModel(orders, 12);

        readModel.Total.Should().Be(12);
        var item = readModel.Items.Should().ContainSingle().Subject;
        item.Id.Should().Be(orderId);
        item.OrderDate.Should().Be(orderDate);
        item.OrderNumber.Should().Be("O-1");
        item.Cost.Should().Be(100);
        item.Address.Should().Be("Kyivska, Kyiv");
        item.CustomerName.Should().Be("Ann Smith");
        item.ProductsCount.Should().Be(3);
        item.IsWholesale.Should().BeTrue();
    }
}
