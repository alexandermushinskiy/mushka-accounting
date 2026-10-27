using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Exhibition;
using Mushka.Api.Converters.Exhibitions;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Api.Tests.Converters.Exhibitions;

public class ExhibitionRequestConverterTests
{
    private readonly ExhibitionRequestConverter _converter = new ExhibitionRequestConverter();

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var productId = Guid.NewGuid();
        var request = Request(productId);

        var command = _converter.ToCreateCommand(request);

        command.Exhibition.Name.Should().Be("Fair");
        command.Exhibition.FromDate.Should().Be(new DateTime(2024, 6, 1));
        command.Exhibition.ToDate.Should().Be(new DateTime(2024, 6, 3));
        command.Exhibition.City.Should().Be("Kyiv");
        command.Exhibition.ParticipationCost.Should().Be(100);
        command.Exhibition.ParticipationCostMethod.Should().Be(PaymentMethod.Cash);
        command.Exhibition.AccommodationCost.Should().Be(40);
        command.Exhibition.AccommodationCostMethod.Should().Be(PaymentMethod.TransferToCard);
        command.Exhibition.FareCost.Should().Be(15);
        command.Exhibition.FareCostMethod.Should().Be(PaymentMethod.LiqPay);
        command.Exhibition.Profit.Should().Be(80);
        command.Exhibition.Notes.Should().Be("note");
        command.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { ProductId = productId, Quantity = 3, UnitPrice = 12m, CostPrice = 6m });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = Request(Guid.NewGuid());

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Exhibition.Name.Should().Be("Fair");
        command.Products.Should().ContainSingle();
    }

    [Fact]
    public void ToCreateCommand_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var request = Request(Guid.NewGuid());
        request.Products = null;

        var command = _converter.ToCreateCommand(request);

        command.Products.Should().BeEmpty();
    }

    private static ExhibitionRequestModel Request(Guid productId) =>
        new ExhibitionRequestModel
        {
            Name = "Fair",
            FromDate = new DateTime(2024, 6, 1),
            ToDate = new DateTime(2024, 6, 3),
            City = "Kyiv",
            ParticipationCost = 100,
            ParticipationCostMethod = PaymentMethod.Cash,
            AccommodationCost = 40,
            AccommodationCostMethod = PaymentMethod.TransferToCard,
            FareCost = 15,
            FareCostMethod = PaymentMethod.LiqPay,
            Profit = 80,
            Notes = "note",
            Products = new[]
            {
                new ExhibitionProductRequestModel
                {
                    ProductId = productId,
                    Quantity = 3,
                    UnitPrice = 12,
                    CostPrice = 6
                }
            }
        };
}
