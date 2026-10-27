using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Converters;

public class GetDefaultExhibitionProductsConverterTests
{
    private readonly GetDefaultExhibitionProductsConverter _converter = new GetDefaultExhibitionProductsConverter();

    [Fact]
    public void ToReadModel_MapsProducts()
    {
        var productId = Guid.NewGuid();
        var products = new List<ExhibitionProduct>
        {
            new ExhibitionProduct
            {
                ProductId = productId,
                Quantity = 1,
                UnitPrice = 0,
                CostPrice = 4.5m,
                Product = new Product
                {
                    Name = "Postcard",
                    VendorCode = "PC-1",
                    Size = new Size { Name = "One" }
                }
            }
        };

        var readModel = _converter.ToReadModel(products);

        readModel.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Id = productId,
                Name = "Postcard",
                VendorCode = "PC-1",
                SizeName = "One",
                Quantity = 1,
                UnitPrice = 0m,
                CostPrice = 4.5m
            });
    }
}
