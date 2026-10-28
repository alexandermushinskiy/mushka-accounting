using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class SelectProductsConverterTests
{
    private readonly SelectProductsConverter _converter = new SelectProductsConverter();

    [Fact]
    public void ToReadModel_MapsProductCategoryAndSize()
    {
        var productId = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product
            {
                Id = productId,
                Name = "Socks",
                VendorCode = "SK-1",
                RecommendedPrice = 10,
                Quantity = 3,
                IsArchival = true,
                Category = new Category { Name = "Clothes" },
                Size = new Size { Name = "M" }
            }
        };

        var item = _converter.ToReadModel(products).Items.Should().ContainSingle().Subject;

        item.Id.Should().Be(productId);
        item.CategoryName.Should().Be("Clothes");
        item.SizeName.Should().Be("M");
        item.Quantity.Should().Be(3);
        item.IsArchival.Should().BeTrue();
    }
}
