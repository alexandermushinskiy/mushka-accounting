using System;
using FluentAssertions;
using Mushka.Api.ClientModels.CorporateOrder;
using Mushka.Api.Converters.CorporateOrders;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Api.Tests.Converters.CorporateOrders;

public class CorporateOrderRequestConverterTests
{
    private readonly CorporateOrderRequestConverter _converter = new CorporateOrderRequestConverter();

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var createdOn = new DateTime(2024, 3, 1);
        var request = Request(createdOn);

        var command = _converter.ToCreateCommand(request);

        command.Order.CreatedOn.Should().Be(createdOn);
        command.Order.OrderNumber.Should().Be("CO-1");
        command.Order.Cost.Should().Be(250);
        command.Order.CostMethod.Should().Be(PaymentMethod.Cash);
        command.Order.Prepayment.Should().Be(50);
        command.Order.PrepaymentMethod.Should().Be(PaymentMethod.TransferToCard);
        command.Order.DeliveryCost.Should().Be(20);
        command.Order.DeliveryCostMethod.Should().Be(PaymentMethod.LiqPay);
        command.Order.Tax.Should().Be(5);
        command.Order.Profit.Should().Be(80);
        command.Order.Region.Should().Be("Kyivska");
        command.Order.City.Should().Be("Kyiv");
        command.Order.CompanyName.Should().Be("Acme");
        command.Order.ContactPerson.Should().Be("Ann");
        command.Order.Phone.Should().Be("123");
        command.Order.Email.Should().Be("ann@example.com");
        command.Order.Notes.Should().Be("note");
        command.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { Name = "Socks", Quantity = 3, UnitPrice = 40m, CostPrice = 15m });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = Request(new DateTime(2024, 4, 2));

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Order.OrderNumber.Should().Be("CO-1");
        command.Products.Should().ContainSingle();
    }

    [Fact]
    public void ToCreateCommand_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var request = new CorporateOrderRequestModel
        {
            OrderNumber = "CO-1",
            CompanyName = "Acme"
        };

        var command = _converter.ToCreateCommand(request);

        command.Products.Should().BeEmpty();
    }

    private static CorporateOrderRequestModel Request(DateTime createdOn) =>
        new CorporateOrderRequestModel
        {
            CreatedOn = createdOn,
            OrderNumber = "CO-1",
            Cost = 250,
            CostMethod = PaymentMethod.Cash,
            Prepayment = 50,
            PrepaymentMethod = PaymentMethod.TransferToCard,
            DeliveryCost = 20,
            DeliveryCostMethod = PaymentMethod.LiqPay,
            Tax = 5,
            Profit = 80,
            Region = "Kyivska",
            City = "Kyiv",
            CompanyName = "Acme",
            ContactPerson = "Ann",
            Phone = "123",
            Email = "ann@example.com",
            Notes = "note",
            Products = new[]
            {
                new CorporateOrderProductRequestModel
                {
                    Name = "Socks",
                    Quantity = 3,
                    UnitPrice = 40,
                    CostPrice = 15
                }
            }
        };
}
