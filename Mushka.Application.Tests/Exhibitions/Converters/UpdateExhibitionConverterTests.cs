using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Converters;

public class UpdateExhibitionConverterTests
{
    private readonly UpdateExhibitionConverter _converter = new UpdateExhibitionConverter();

    [Fact]
    public void ToExhibition_UsesCommandId_AndMapsProducts()
    {
        var exhibitionId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateExhibitionCommand(
            exhibitionId,
            new ExhibitionData("Fair", new DateTime(2024, 6, 1), new DateTime(2024, 6, 3), "Lviv", 20, PaymentMethod.LiqPay, null, null, null, null, 5, "note"),
            new List<ExhibitionProductData> { new ExhibitionProductData(productId, 2, 9, 3) });

        var exhibition = _converter.ToExhibition(command);

        exhibition.Id.Should().Be(exhibitionId);
        exhibition.Name.Should().Be("Fair");
        exhibition.City.Should().Be("Lviv");
        exhibition.ParticipationCostMethod.Should().Be(PaymentMethod.LiqPay);
        exhibition.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ExhibitionProduct
            {
                ExhibitionId = exhibitionId,
                ProductId = productId,
                Quantity = 2,
                UnitPrice = 9,
                CostPrice = 3
            });
    }

    [Fact]
    public void ToExhibition_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var command = new UpdateExhibitionCommand(
            Guid.NewGuid(),
            new ExhibitionData("Fair", DateTime.UtcNow, DateTime.UtcNow, "Kyiv", 1, PaymentMethod.Cash, null, null, null, null, 1, null),
            null);

        var exhibition = _converter.ToExhibition(command);

        exhibition.Products.Should().BeEmpty();
    }
}
