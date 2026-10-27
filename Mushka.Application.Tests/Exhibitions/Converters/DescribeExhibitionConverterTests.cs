using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Converters;

public class DescribeExhibitionConverterTests
{
    private readonly DescribeExhibitionConverter _converter = new DescribeExhibitionConverter();

    [Fact]
    public void ToReadModel_MapsExhibitionAndProducts()
    {
        var exhibitionId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var exhibition = new Exhibition
        {
            Id = exhibitionId,
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
            Products = new List<ExhibitionProduct>
            {
                new ExhibitionProduct
                {
                    ProductId = productId,
                    Quantity = 3,
                    UnitPrice = 12,
                    CostPrice = 6,
                    Product = new Product
                    {
                        Name = "Socks",
                        VendorCode = "SK-1",
                        Size = new Size { Name = "M" }
                    }
                }
            }
        };

        var readModel = _converter.ToReadModel(exhibition);

        readModel.Exhibition.Id.Should().Be(exhibitionId);
        readModel.Exhibition.Name.Should().Be("Fair");
        readModel.Exhibition.City.Should().Be("Kyiv");
        readModel.Exhibition.ParticipationCost.Should().Be(100);
        readModel.Exhibition.ParticipationCostMethod.Should().Be(PaymentMethod.Cash);
        readModel.Exhibition.AccommodationCost.Should().Be(40);
        readModel.Exhibition.FareCostMethod.Should().Be(PaymentMethod.LiqPay);
        readModel.Exhibition.Profit.Should().Be(80);
        readModel.Exhibition.Notes.Should().Be("note");
        readModel.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Id = productId,
                Name = "Socks",
                VendorCode = "SK-1",
                SizeName = "M",
                Quantity = 3,
                UnitPrice = 12m,
                CostPrice = 6m
            });
    }

    [Fact]
    public void ToReadModel_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var exhibition = new Exhibition { Id = Guid.NewGuid(), Products = null };

        var readModel = _converter.ToReadModel(exhibition);

        readModel.Products.Should().BeEmpty();
    }
}
