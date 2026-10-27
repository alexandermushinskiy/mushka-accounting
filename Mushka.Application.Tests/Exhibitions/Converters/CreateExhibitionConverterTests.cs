using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Converters;

public class CreateExhibitionConverterTests
{
    private readonly Guid _exhibitionId = Guid.Parse("00000000-0000-0000-0000-000000000021");
    private readonly CreateExhibitionConverter _converter;

    public CreateExhibitionConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_exhibitionId);
        _converter = new CreateExhibitionConverter(guidProvider.Object);
    }

    [Fact]
    public void ToExhibition_MapsCommand_AndAssignsGeneratedId()
    {
        var productId = Guid.NewGuid();
        var fromDate = new DateTime(2024, 6, 1);
        var toDate = new DateTime(2024, 6, 3);
        var command = new CreateExhibitionCommand(
            new ExhibitionData(
                "Fair",
                fromDate,
                toDate,
                "Kyiv",
                100,
                PaymentMethod.Cash,
                40,
                PaymentMethod.TransferToCard,
                15,
                PaymentMethod.LiqPay,
                80,
                "note"),
            new List<ExhibitionProductData>
            {
                new ExhibitionProductData(productId, 4, 12, 6)
            });

        var exhibition = _converter.ToExhibition(command);

        exhibition.Id.Should().Be(_exhibitionId);
        exhibition.Name.Should().Be("Fair");
        exhibition.FromDate.Should().Be(fromDate);
        exhibition.ToDate.Should().Be(toDate);
        exhibition.City.Should().Be("Kyiv");
        exhibition.ParticipationCost.Should().Be(100);
        exhibition.ParticipationCostMethod.Should().Be(PaymentMethod.Cash);
        exhibition.AccommodationCost.Should().Be(40);
        exhibition.AccommodationCostMethod.Should().Be(PaymentMethod.TransferToCard);
        exhibition.FareCost.Should().Be(15);
        exhibition.FareCostMethod.Should().Be(PaymentMethod.LiqPay);
        exhibition.Profit.Should().Be(80);
        exhibition.Notes.Should().Be("note");
        exhibition.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ExhibitionProduct
            {
                ExhibitionId = _exhibitionId,
                ProductId = productId,
                Quantity = 4,
                UnitPrice = 12,
                CostPrice = 6
            });
    }

    [Fact]
    public void ToExhibition_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var command = new CreateExhibitionCommand(
            new ExhibitionData("Fair", DateTime.UtcNow, DateTime.UtcNow, "Kyiv", 1, PaymentMethod.Cash, null, null, null, null, 1, null),
            null);

        var exhibition = _converter.ToExhibition(command);

        exhibition.Products.Should().BeEmpty();
    }
}
