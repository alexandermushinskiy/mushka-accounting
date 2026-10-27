using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Converters;

public class SearchExhibitionsConverterTests
{
    private readonly SearchExhibitionsConverter _converter = new SearchExhibitionsConverter();

    [Fact]
    public void ToReadModel_MapsExhibitionsAndSumsProductQuantities()
    {
        var exhibitionId = Guid.NewGuid();
        var fromDate = new DateTime(2024, 6, 1);
        var toDate = new DateTime(2024, 6, 3);
        var exhibitions = new List<Exhibition>
        {
            new Exhibition
            {
                Id = exhibitionId,
                Name = "Fair",
                FromDate = fromDate,
                ToDate = toDate,
                City = "Kyiv",
                ParticipationCost = 100,
                Products = new List<ExhibitionProduct>
                {
                    new ExhibitionProduct { Quantity = 2 },
                    new ExhibitionProduct { Quantity = 5 }
                }
            }
        };

        var readModel = _converter.ToReadModel(exhibitions);

        readModel.Total.Should().Be(1);
        var item = readModel.Items.Should().ContainSingle().Subject;
        item.Id.Should().Be(exhibitionId);
        item.Name.Should().Be("Fair");
        item.FromDate.Should().Be(fromDate);
        item.ToDate.Should().Be(toDate);
        item.City.Should().Be("Kyiv");
        item.ParticipationCost.Should().Be(100);
        item.ProductsCount.Should().Be(7);
    }

    [Fact]
    public void ToReadModel_WhenProductsAreNull_UsesZeroProductsCount()
    {
        var exhibitions = new List<Exhibition>
        {
            new Exhibition { Name = "Fair", Products = null }
        };

        var readModel = _converter.ToReadModel(exhibitions);

        readModel.Items.Should().ContainSingle().Which.ProductsCount.Should().Be(0);
    }
}
