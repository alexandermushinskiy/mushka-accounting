using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Orders.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Orders.Converters;

public class DescribeOrderConverterTests
{
    private readonly DescribeOrderConverter _converter = new DescribeOrderConverter();

    [Fact]
    public void ToReadModel_MapsOrderCustomerAndProducts()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var orderDate = new DateTime(2024, 3, 1);
        var order = new Order
        {
            Id = orderId,
            OrderDate = orderDate,
            Number = "O-1",
            Cost = 250,
            CostMethod = PaymentMethod.Cash,
            Discount = null,
            Profit = 80,
            IsWholesale = true,
            Notes = "note",
            Region = "Kyivska",
            City = "Kyiv",
            Customer = new Customer
            {
                Id = customerId,
                FirstName = "Ann",
                LastName = "Smith",
                Phone = "123",
                Email = "ann@example.com"
            },
            Products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = productId,
                    Quantity = 2,
                    UnitPrice = 40,
                    CostPrice = 15,
                    Product = new Product
                    {
                        Name = "Socks",
                        VendorCode = "S-1",
                        Size = new Size { Name = "M" }
                    }
                }
            }
        };

        var readModel = _converter.ToReadModel(order);

        readModel.Order.Id.Should().Be(orderId);
        readModel.Order.OrderDate.Should().Be(orderDate);
        readModel.Order.Number.Should().Be("O-1");
        readModel.Order.Cost.Should().Be(250);
        readModel.Order.CostMethod.Should().Be(PaymentMethod.Cash);
        readModel.Order.Discount.Should().Be(0);
        readModel.Order.Profit.Should().Be(80);
        readModel.Order.IsWholesale.Should().BeTrue();
        readModel.Order.Region.Should().Be("Kyivska");
        readModel.Order.City.Should().Be("Kyiv");
        readModel.Customer.Id.Should().Be(customerId);
        readModel.Customer.FirstName.Should().Be("Ann");
        readModel.Customer.LastName.Should().Be("Smith");
        readModel.Customer.Phone.Should().Be("123");
        readModel.Customer.Email.Should().Be("ann@example.com");
        readModel.Products.Should().ContainSingle(product =>
            product.Id == productId &&
            product.Name == "Socks" &&
            product.VendorCode == "S-1" &&
            product.SizeName == "M" &&
            product.Quantity == 2 &&
            product.UnitPrice == 40 &&
            product.CostPrice == 15);
    }

    [Fact]
    public void ToReadModel_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var order = new Order
        {
            Customer = new Customer(),
            Products = null
        };

        var readModel = _converter.ToReadModel(order);

        readModel.Products.Should().BeEmpty();
    }
}
