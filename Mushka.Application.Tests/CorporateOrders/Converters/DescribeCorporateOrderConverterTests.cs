using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Converters;

public class DescribeCorporateOrderConverterTests
{
    private readonly DescribeCorporateOrderConverter _converter = new DescribeCorporateOrderConverter();

    [Fact]
    public void ToReadModel_MapsOrderAndProducts()
    {
        var orderId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 3, 1);
        var order = new CorporateOrder
        {
            Id = orderId,
            CreatedOn = createdOn,
            Number = "CO-1",
            Cost = 250,
            CostMethod = PaymentMethod.Cash,
            Prepayment = 50,
            PrepaymentMethod = PaymentMethod.TransferToCard,
            DeliveryCost = 20,
            DeliveryCostMethod = PaymentMethod.LiqPay,
            Tax = 5,
            Profit = 80,
            Notes = "note",
            CompanyName = "Acme",
            ContactPerson = "Ann",
            Phone = "123",
            Email = "ann@example.com",
            Region = "Kyivska",
            City = "Kyiv",
            Products = new List<CorporateOrderProduct>
            {
                new CorporateOrderProduct
                {
                    Name = "Socks",
                    Quantity = 3,
                    UnitPrice = 40,
                    CostPrice = 15
                }
            }
        };

        var readModel = _converter.ToReadModel(order);

        readModel.Order.Id.Should().Be(orderId);
        readModel.Order.CreatedOn.Should().Be(createdOn);
        readModel.Order.OrderNumber.Should().Be("CO-1");
        readModel.Order.Cost.Should().Be(250);
        readModel.Order.CostMethod.Should().Be(PaymentMethod.Cash);
        readModel.Order.Prepayment.Should().Be(50);
        readModel.Order.PrepaymentMethod.Should().Be(PaymentMethod.TransferToCard);
        readModel.Order.DeliveryCost.Should().Be(20);
        readModel.Order.DeliveryCostMethod.Should().Be(PaymentMethod.LiqPay);
        readModel.Order.Tax.Should().Be(5);
        readModel.Order.Profit.Should().Be(80);
        readModel.Order.Notes.Should().Be("note");
        readModel.Order.CompanyName.Should().Be("Acme");
        readModel.Order.ContactPerson.Should().Be("Ann");
        readModel.Order.Phone.Should().Be("123");
        readModel.Order.Email.Should().Be("ann@example.com");
        readModel.Order.Region.Should().Be("Kyivska");
        readModel.Order.City.Should().Be("Kyiv");
        readModel.Products.Should().ContainSingle(product =>
            product.Name == "Socks" &&
            product.Quantity == 3 &&
            product.UnitPrice == 40 &&
            product.CostPrice == 15);
    }

    [Fact]
    public void ToReadModel_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var order = new CorporateOrder
        {
            Id = Guid.NewGuid(),
            Number = "CO-1",
            Products = null
        };

        var readModel = _converter.ToReadModel(order);

        readModel.Products.Should().BeEmpty();
    }
}
